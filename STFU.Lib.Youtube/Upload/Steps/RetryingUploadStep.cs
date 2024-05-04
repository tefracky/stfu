using System;
using System.Threading;
using System.Threading.Tasks;
using STFU.Lib.Youtube.Interfaces.Model;
using STFU.Lib.Youtube.Interfaces.Model.Enums;

namespace STFU.Lib.Youtube.Upload.Steps
{
	public class RetryingUploadStep<T> : AbstractUploadStep where T : AbstractUploadStep
	{
		private T Step { get; set; }

		public int WaitInterval { get; private set; }

		public override double Progress => Step != null ? Step.Progress : 0;

		public new bool FinishedSuccessful => Step != null && Step.FinishedSuccessful;

		public RetryingUploadStep(IYoutubeJob job, int waitInterval = 90000)
			: base(job)
		{
			Step = (T)Activator.CreateInstance(typeof(T), job);
			Logger.Info($"Creating retrying upload step for step '{Step.GetType().Name}'");
			WaitInterval = waitInterval;
		}

		internal override void Run()
		{
			int tries = 0;
			var lastProgress = Status.Progress = 0.0;

			OnStepStateChanged(UploadStepState.NotStarted, UploadStepState.Running);
			CancellationTokenSource = new CancellationTokenSource();

			while (tries < 10 && !Step.FinishedSuccessful && !CancellationTokenSource.IsCancellationRequested)
			{
				tries++;
				Logger.Info($"Running step '{Step.GetType().Name}' - attempt number: {tries}");

				try
				{
					Step.Run();

					if (Step.FinishedSuccessful || CancellationTokenSource.IsCancellationRequested)
					{
						Logger.Info($"Step '{Step.GetType().Name}' finished successfully");
					}
					else if (Step.Status.QuotaReached)
					{
						Logger.Warn($"Step '{Step.GetType().Name}' couldn't finish because the Youtube Quota was reached - waiting until retry");
						WaitForQuotaBeforeRetry();
					}
					else
					{
						Logger.Warn($"Step '{Step.GetType().Name}' did not finish successfully - waiting before retrying");
						WaitBeforeRetry();
					}
				}
				catch (Exception ex)
				{
					Logger.Error($"Step '{Step.GetType().Name}' failed because of an exception", ex);
					WaitBeforeRetry();
				}

				if (lastProgress < Status.Progress)
				{
					Logger.Info($"Step '{Step.GetType().Name}' increased progress since last attempt - resetting attempt count");
					lastProgress = Status.Progress;
					tries = 0;
				}
			}

			if (Step.FinishedSuccessful)
			{
				OnStepFinished();
			}
			else if (!CancellationTokenSource.IsCancellationRequested)
			{
				OnStepStateChanged(UploadStepState.Running, UploadStepState.Error);
			}
		}

		private void WaitBeforeRetry()
		{
			OnStepStateChanged(UploadStepState.Running, UploadStepState.Broke);
			Thread.Sleep(WaitInterval);
			OnStepStateChanged(UploadStepState.Broke, UploadStepState.Running);
		}

		private void WaitForQuotaBeforeRetry()
		{
			OnStepStateChanged(UploadStepState.Running, UploadStepState.QuotaReached);
			Random r = new Random();

			DateTime retryTime;
			if (DateTime.Now.Hour > 9)
			{
				retryTime = DateTime.Now.Date.AddDays(1).AddHours(9).AddMinutes(r.Next(1, 60));
			}
			else
			{
				retryTime = DateTime.Now.Date.AddHours(9).AddMinutes(r.Next(1, 60));
			}

			Status.WaitTime = retryTime.Subtract(DateTime.Now);

			Thread.Sleep(retryTime.Subtract(DateTime.Now));
			Status.QuotaReached = false;
			Status.WaitTime = default;
			OnStepStateChanged(UploadStepState.QuotaReached, UploadStepState.Running);
		}

		public override void RefreshDurationAndSpeed()
		{
			Step.RefreshDurationAndSpeed();
		}

		public override void Cancel()
		{
			if (RunningTask != null && RunningTask.Status == TaskStatus.Running)
			{
				Logger.Info($"Canceling step '{Step.GetType().Name}'");
				CancellationTokenSource.Cancel();
				Step?.Cancel();
			}
		}
	}
}
