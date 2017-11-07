﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using API.Models;
using Domain;

namespace API.Controllers
{
    //[Authorize]
    [Authorize(Roles = "User")]
    public class LeaguesController : ApiController
    {
        private readonly DataContext _db = new DataContext();

        public async Task<IHttpActionResult> GetLeagues()
        {
            var leagues = await _db.Leagues.ToArrayAsync();
            var list = new List<LeagueResponse>();
            foreach (var league in leagues)
            {
                list.Add(new LeagueResponse
                {
                    LeagueId = league.LeagueId,
                    LogoL = league.LogoL,
                    Name = league.Name,
                    Teams = league.Teams.ToList(),
                });
            }

            return Ok(list);
        }

        [Authorize]
        [ResponseType(typeof(League))]
        public async Task<IHttpActionResult> GetLeague(int id)
        {
            var league = await _db.Leagues.FindAsync(id);
            if (league == null)
            {
                return NotFound();
            }

            return Ok(league);
        }
        //// GET: api/Leagues/5
        //[ResponseType(typeof(League))]
        //public async Task<IHttpActionResult> GetLeague(int id)
        //{
        //    var league = await db.Leagues.FindAsync(id);
        //    if (league == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(league);
        //}

        //// PUT: api/Leagues/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutLeague(int id, League league)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != league.LeagueId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(league).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!LeagueExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Leagues
        //[ResponseType(typeof(League))]
        //public async Task<IHttpActionResult> PostLeague(League league)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Leagues.Add(league);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = league.LeagueId }, league);
        //}

        //// DELETE: api/Leagues/5
        //[ResponseType(typeof(League))]
        //public async Task<IHttpActionResult> DeleteLeague(int id)
        //{
        //    var league = await db.Leagues.FindAsync(id);
        //    if (league == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Leagues.Remove(league);
        //    await db.SaveChangesAsync();

        //    return Ok(league);
        //}
        // PUT: api/Leagues/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLeague(int id, League league)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != league.LeagueId)
            {
                return BadRequest();
            }

            _db.Entry(league).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeagueExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Leagues
        [ResponseType(typeof(League))]
        public async Task<IHttpActionResult> PostLeague(League league)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Leagues.Add(league);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = league.LeagueId }, league);
        }

        // DELETE: api/Leagues/5
        [ResponseType(typeof(League))]
        public async Task<IHttpActionResult> DeleteLeague(int id)
        {
            var league = await _db.Leagues.FindAsync(id);
            if (league == null)
            {
                return NotFound();
            }

            _db.Leagues.Remove(league);
            await _db.SaveChangesAsync();

            return Ok(league);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LeagueExists(int id)
        {
            return _db.Leagues.Count(e => e.LeagueId == id) > 0;
        }
    }
}