using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace STFU.Lib.Youtube.Upload.Steps
{
	public class ThrottledReadStream : Stream
	{
		private static ILog Logger { get; set; } = LogManager.GetLogger(nameof(ThrottledReadStream));

		private delegate void ThrottleEnabledChangedEventHandler();
		private delegate void ThrottleChangedEventHandler();

		private static bool _shouldThrottle = false;
		public static bool ShouldThrottle
		{
			get
			{
				return _shouldThrottle;
			}
			set
			{
				if (_shouldThrottle != value)
				{
					Logger.Info($"Throttling is now {(value ? "enabled" : "disabled")}");

					_shouldThrottle = value;
					_shouldThrottleChanged?.Invoke();
				}
			}
		}

		private static long _globalLimit = 1_000_000;
		public static long GlobalLimit
		{
			get
			{
				return _globalLimit;
			}
			set
			{
				if (_globalLimit != value)
				{
					Logger.Info($"Global upload throttle was set to {value}");

					_globalLimit = value;
					_throttleChanged?.Invoke();
				}
			}
		}

		private long DefinedLimit { get; set; } = GlobalLimit;

		private static ThrottleEnabledChangedEventHandler _shouldThrottleChanged;
		private static ThrottleChangedEventHandler _throttleChanged;

		private static int KByteModifier { get; set; } = 1000;

        readonly Stream baseStream = null;
        readonly Stopwatch watch = Stopwatch.StartNew();
		long totalBytesRead = 0;

		public ThrottledReadStream(Stream incommingStream)
		{
			baseStream = incommingStream;
			_shouldThrottleChanged += ResetThrottle;
			_throttleChanged += ResetThrottleIfActive;
		}

		~ThrottledReadStream()
		{
			Logger.Info($"Throttled read stream gets destructed");

			_shouldThrottleChanged -= ResetThrottle;
			_throttleChanged -= ResetThrottleIfActive;
		}

		private void ResetThrottle()
		{
			totalBytesRead = 0;
			watch.Restart();
			DefinedLimit = GlobalLimit;
		}

		private void ResetThrottleIfActive()
		{
			if (ShouldThrottle)
			{
				ResetThrottle();
			}
		}

		public override bool CanRead
		{
			get { return baseStream.CanRead; }
		}

		public override bool CanSeek
		{
			get { return baseStream.CanSeek; }
		}

		public override bool CanWrite
		{
			get { return false; }
		}

		public override void Flush()
		{
		}

		public override long Length
		{
			get { return baseStream.Length; }
		}

		public override long Position
		{
			get
			{
				return baseStream.Position;
			}
			set
			{
				baseStream.Position = value;
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (ShouldThrottle)
			{
				var newcount = GetBytesToReturn(count);
				int read = baseStream.Read(buffer, offset, newcount);
				Interlocked.Add(ref totalBytesRead, read);
				return read;
			}
			else
			{
				return baseStream.Read(buffer, offset, count);
			}
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return baseStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
		}

		int GetBytesToReturn(int count)
		{
			return GetBytesToReturnAsync(count).Result;
		}

		async Task<int> GetBytesToReturnAsync(int count)
		{
			if (DefinedLimit <= 0)
			{
				return count;
			}

			long canSend = (long)(watch.ElapsedMilliseconds * (DefinedLimit / 1000.0));

			int diff = (int)(canSend - totalBytesRead);

			if (diff <= 0)
			{
				var waitInSec = ((diff * -1.0) / (DefinedLimit));

				await Task.Delay((int)(waitInSec * 1000)).ConfigureAwait(false);
			}

			if (diff >= count)
			{
				return count;
			}

			return diff > 0 ? diff : Math.Min(KByteModifier, count);
		}
	}
}
