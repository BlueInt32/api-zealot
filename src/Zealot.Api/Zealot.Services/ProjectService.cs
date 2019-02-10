using Zealot.Repository;

namespace Zealot.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        public ProjectService()
        {

        }

        public void SaveProject()
        {
            throw new System.NotImplementedException();
        }
    }
}
