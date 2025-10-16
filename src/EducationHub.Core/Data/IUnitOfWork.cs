namespace EducationHub.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
