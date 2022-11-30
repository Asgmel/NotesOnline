using NotesOnline.Dtos;

namespace NotesOnline.Web.Services.Contracts
{
    public interface IUserService
    {
        public Task<bool> Login(UserAuthDto userAuthDto);

        public Task Logout();

        public Task<bool> Register(UserCreateDto userCreateDto);
    }
}
