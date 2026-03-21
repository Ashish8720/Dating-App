using Dating_App.Data;
using Dating_App.Entities;
using Dating_App.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dating_App.Repository
{
    public class MemberRepository(AppDbContext context) : IMemberRepository
    {
        public async Task<Member?> GetMemberByIdAsync(string id)
        {
            return await context.Members.FindAsync(id);

        }

        public async Task<IReadOnlyList<Member>> GetMembersAsync()
        {
            return await context.Members.ToListAsync();
        }

        // Get photos by member ID
        public async Task<IReadOnlyList<Photo>> GetPhotosByMemberIdAsync(string memberId)
        {
            return await context.Members
                .Where(m => m.Id == memberId)
                .SelectMany(m => m.Photos)
                .ToListAsync();
        }

        // Save all changes to the database
        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Update(Member member)
        {
            // Mark the member entity as modified
            context.Entry(member).State = EntityState.Modified;
        }
    }
}
