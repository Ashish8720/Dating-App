using Dating_App.Entities;
using Dating_App.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dating_App.Controllers
{
    [Authorize]
    public class MemberController(IMemberRepository memberRepository ) : BaseController
    {
        /// <summary>
        /// Retrieves a read-only list of all members.
        /// </summary>
        /// <returns>﻿An <see cref="ActionResult{T}"/> containing a read-only list of <see cref="Member"/> objects.  The list is
        /// empty if no members are found.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers()
        {
            var members = await memberRepository.GetMembersAsync();
            return Ok(members);
        }

        /// <summary>
        /// Retrieves the member with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the member to retrieve. Cannot be null or empty.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing the <see cref="Member"/> if found; otherwise, a 404 Not Found
        /// response if no member exists with the specified identifier.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(string id)
        {
            var member = await memberRepository.GetMemberByIdAsync(id);
            if (member == null) return NotFound("Member not found");
            return Ok(member);
        }


        /// <summary>
        /// Retrieves the collection of photos associated with the specified member.
        /// </summary>
        /// <remarks>This endpoint is typically used to display or manage the photos belonging to a
        /// particular member.</remarks>
        /// <param name="memberId">The unique identifier of the member whose photos are to be retrieved. Cannot be null or empty.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a read-only list of <see cref="Photo"/> objects for the
        /// specified member. Returns an empty list if the member has no photos.</returns>
        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string memberId)
        {
            var photos = await memberRepository.GetPhotosByMemberIdAsync(memberId);
            return Ok(photos);
        }
          


    }
}
