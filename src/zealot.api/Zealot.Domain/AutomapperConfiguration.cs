using AutoMapper;
using Zealot.Domain.Models;
using Zealot.Domain.Objects;

namespace Zealot.Domain
{
    public class AutomapperConfigurationFactory
    {
        public MapperConfiguration BuildConfiguration()
        {
            var automapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<ProjectModel, Project>();
                cfg.CreateMap<SubTreeModel, Node>();
            });
            return automapperConfig;
        }
    }
}
