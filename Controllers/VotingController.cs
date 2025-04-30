using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleVotingApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace SimpleVotingApp.Controllers
{
    [Authorize] // Require login for the entire controller
    public class VotingController : Controller
    {
        private readonly VotingContext _context;

        public VotingController(VotingContext context)
        {
            _context = context;
        }

        // GET: /Voting/Vote
        public IActionResult Vote()
        {
            var candidates = _context.Candidates.ToList();
            return View(candidates);
        }

        [HttpPost]
        public IActionResult Vote(int selectedCandidateId)
        {
            var candidates = _context.Candidates.ToList();
            var selectedCandidate = _context.Candidates.FirstOrDefault(c => c.Id == selectedCandidateId);

            if (selectedCandidate != null)
            {
                var vote = new Vote { CandidateId = selectedCandidateId };
                _context.Votes.Add(vote);
                _context.SaveChanges();
                return RedirectToAction("ThankYou");
            }
            else
            {
                ModelState.AddModelError("", "The selected candidate does not exist.");
                return View(candidates);
            }
        }

        public IActionResult ThankYou()
        {
            return View();
        }

        public IActionResult Manage()
        {
            var candidates = _context.Candidates.ToList();
            return View(candidates);
        }

        public IActionResult Edit(int id)
        {
            var candidate = _context.Candidates.FirstOrDefault(c => c.Id == id);
            if (candidate == null)
            {
                return NotFound();
            }
            return View(candidate);
        }

        [HttpPost]
        public IActionResult Edit(int id, string name)
        {
            var candidate = _context.Candidates.FirstOrDefault(c => c.Id == id);
            if (candidate == null)
            {
                return NotFound();
            }

            candidate.Name = name;
            _context.SaveChanges();
            return RedirectToAction("Manage");
        }

        [HttpPost]
        public IActionResult EditInline(int id, string name)
        {
            var candidate = _context.Candidates.FirstOrDefault(c => c.Id == id);
            if (candidate != null)
            {
                candidate.Name = name;
                _context.SaveChanges();
            }
            return RedirectToAction("Manage");
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            _context.Candidates.Add(new Candidate { Name = name });
            _context.SaveChanges();
            return RedirectToAction("Manage");
        }

        public IActionResult Remove(int id)
        {
            var candidate = _context.Candidates.Find(id);
            if (candidate != null)
            {
                _context.Candidates.Remove(candidate);
                _context.SaveChanges();
            }
            return RedirectToAction("Manage");
        }
    }
}
