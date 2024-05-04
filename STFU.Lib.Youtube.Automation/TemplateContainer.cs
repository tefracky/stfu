using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using log4net;
using STFU.Lib.Youtube.Automation.Interfaces;
using STFU.Lib.Youtube.Automation.Interfaces.Model;
using STFU.Lib.Youtube.Automation.Templates;

namespace STFU.Lib.Youtube.Automation
{
	public class TemplateContainer : ITemplateContainer
	{
		private static ILog Logger { get; set; } = LogManager.GetLogger(nameof(TemplateContainer));

		private IList<ITemplate> Templates { get; set; } = new List<ITemplate>();

		public IReadOnlyCollection<ITemplate> RegisteredTemplates => new ReadOnlyCollection<ITemplate>(Templates);

		private bool IdIsRegistered(int id)
		{
			Logger.Info($"Has id {id} been registered => {RegisteredTemplates.Any(t => t.Id == id)}");
			return RegisteredTemplates.Any(t => t.Id == id);
		}

		private int GetNextUnusedId()
		{
			int maxId = -1;
			if (RegisteredTemplates.Count > 0)
			{
				maxId = RegisteredTemplates.Max(t => t.Id);
			}
			Logger.Info($"Next unused template id: {maxId + 1}");
			return maxId + 1;
		}

		public void RegisterTemplate(ITemplate template)
		{
			if (IdIsRegistered(template.Id))
			{
				((Template)template).Id = GetNextUnusedId();
			}

			Logger.Info($"Adding template with id: {template.Id} and name: '{template.Name}'");
			Templates.Add(template);
		}

		public void UpdateTemplate(ITemplate template)
		{
			if (RegisteredTemplates.Any(t => t.Id == template.Id))
			{
				var temp = RegisteredTemplates.Single(t => t.Id == template.Id);
				var index = Templates.IndexOf(temp);

				Logger.Info($"Updating template with id: {temp.Id} and name: '{temp.Name}' to new values: '{template}'");
				Templates[index] = template;
			}
		}

		public void UnregisterTemplate(ITemplate template)
		{
			if (RegisteredTemplates.Any(t => t.Id == template.Id))
			{
				var temp = RegisteredTemplates.Single(t => t.Id == template.Id);
				Logger.Info($"Removing template with id: {temp.Id} and name: '{temp.Name}'");
				Templates.Remove(temp);
			}
		}

		public void UnregisterTemplateAt(int index)
		{
			if (RegisteredTemplates.Count > index && Templates[index].Id != 0)
			{
				Logger.Info($"Removing template at index {index} with id: {Templates[index].Id} and name: '{Templates[index].Name}'");
				Templates.RemoveAt(index);
			}
		}

		public void UnregisterAllTemplates()
		{
			Logger.Info($"Removing all templates");
			Templates = Templates.Where(t => t.Id == 0).ToList();
		}

		public void ShiftTemplatePositions(ITemplate first, ITemplate second)
		{
			ITemplate firstToChange = null;
			ITemplate secondToChange = null;
			if (first != null
				&& second != null
				&& (firstToChange = Templates.FirstOrDefault(t => t.Id == first.Id)) != null
				&& (secondToChange = Templates.FirstOrDefault(t => t.Id == second.Id)) != null)
			{
				Logger.Info($"Switching positions of templates with id: {firstToChange.Id} and name: '{firstToChange.Name}' and id: {secondToChange.Id} and name: '{secondToChange.Name}'");
				ShiftTemplatePositionsAt(Templates.IndexOf(firstToChange), Templates.IndexOf(secondToChange));
			}
		}

		public void ShiftTemplatePositionsAt(int firstIndex, int secondIndex)
        {
            if (firstIndex < 0 || secondIndex < 0 || firstIndex >= Templates.Count || secondIndex >= Templates.Count) return;
            Logger.Info($"Switching positions of templates with positions: {firstIndex} and {secondIndex}");

            (Templates[firstIndex], Templates[secondIndex]) = (Templates[secondIndex], Templates[firstIndex]);
        }
	}
}
