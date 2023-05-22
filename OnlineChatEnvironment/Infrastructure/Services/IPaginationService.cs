using OnlineChatEnvironment.Data.Models;

namespace OnlineChatEnvironment.Services
{
    public interface IPaginationService
    {
        public IQueryable<T> Pagination<T>(int pageNumber, IQueryable<T> query) where T : class;

        public int PageCorrection<T>(int pageNumber, IQueryable<T> query) where T : class;
    }
}
