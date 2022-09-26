using CoreApiTest.Models;
using static NuGet.Packaging.PackagingConstants;

namespace CoreApiTest.Resource.Helpers
{
    public class ToUserApiResource
    {
        public UserApiResource DoConvertForModel(User user)
        {
            UserApiResource userApiResource = new()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
            };
            return userApiResource;
        }

        public List<UserApiResource> DoConvertForList(List<User> users)
        {
            List<UserApiResource> userApiResources = new();
            foreach (var user in users)
            {
                userApiResources.Add(DoConvertForModel(user));
            }
            return userApiResources;
        }
    }
}
