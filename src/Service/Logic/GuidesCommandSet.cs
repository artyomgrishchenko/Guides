using Guides.Data.Version1;
using PipServices3.Commons.Commands;
using PipServices3.Commons.Data;
using PipServices3.Commons.Convert;
using PipServices3.Commons.Validate;
using System.Collections.Generic;

namespace Guides.Logic
{
    public class GuidesCommandSet : CommandSet
    {
        private IGuidesController _controller;

        public GuidesCommandSet(IGuidesController controller)
        {
            _controller = controller;

            AddCommand(MakeGetGuidesCommand());
            AddCommand(MakeGetGuideByIdGuidesCommand());
            AddCommand(MakeCreateGuideCommand());
            AddCommand(MakeUpdateGuideCommand());
            AddCommand(MakeDeleteGuideByIdCommand());
        }

        private ICommand MakeGetGuidesCommand()
        {
            return new Command(
                "get_guides",
                new ObjectSchema()
                    .WithOptionalProperty("filter", new FilterParamsSchema())
                    .WithOptionalProperty("paging", new PagingParamsSchema()),
                async (correlationId, parameters) =>
                {
                    var filter = FilterParams.FromValue(parameters.Get("filter"));
                    var paging = PagingParams.FromValue(parameters.Get("paging"));
                    return await _controller.GetGuidesAsync(correlationId, filter, paging);
                });
        }

        private ICommand MakeGetGuideByIdGuidesCommand()
        {
            return new Command(
                "get_guide_by_id",
                new ObjectSchema()
                    .WithRequiredProperty("guide_id", TypeCode.String),
                async (correlationId, parameters) =>
                {
                    var id = parameters.GetAsString("guide_id");
                    return await _controller.GetGuideByIdAsync(correlationId, id);
                });
        }

        private ICommand MakeCreateGuideCommand()
        {
            return new Command(
                "create_guide",
                new ObjectSchema()
                    .WithRequiredProperty("guide", new GuideV1Schema()),
                async (correlationId, parameters) =>
                {
                    var guide = ConvertToGuide(parameters.GetAsObject("guide"));
                    return await _controller.CreateGuideAsync(correlationId, guide);
                });
        }

        private ICommand MakeUpdateGuideCommand()
        {
            return new Command(
               "update_guide",
               new ObjectSchema()
                    .WithRequiredProperty("guide", new GuideV1Schema()),
               async (correlationId, parameters) =>
               {
                   var guide = ConvertToGuide(parameters.GetAsObject("guide"));
                   return await _controller.UpdateGuideAsync(correlationId, guide);
               });
        }

        private ICommand MakeDeleteGuideByIdCommand()
        {
            return new Command(
               "delete_guide_by_id",
               new ObjectSchema()
                   .WithRequiredProperty("guide_id", TypeCode.String),
               async (correlationId, parameters) =>
               {
                   var id = parameters.GetAsString("guide_id");
                   return await _controller.DeleteGuideByIdAsync(correlationId, id);
               });
        }

        private GuideV1 ConvertToGuide(object value)
        {
            return JsonConverter.FromJson<GuideV1>(JsonConverter.ToJson(value));
        }

        private string[] ConvertToStringList(object value)
        {
            return JsonConverter.FromJson<string[]>(JsonConverter.ToJson(value));
        }

    }
}
