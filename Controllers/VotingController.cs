using Microsoft.AspNetCore.Mvc;
using SimpleVotingApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace SimpleVotingApp.Controllers
{
    public class VotingController : Controller
    {
        private readonly VotingContext _context;

        public VotingController(VotingContext context)
        {
            _context = context;
        }

        // Hardcoded list of candidates (for initial setup)
        private static readonly List<Candidate> Candidates = new List<Candidate>
        {
            
        };

        // GET: /Voting/Vote
        public IActionResult Vote()
        {
            // Get the list of candidates from the database
            var candidates = _context.Candidates.ToList();

            // Pass the candidates to the view for display
            return View(candidates);  // Ensure candidates are displayed immediately when the page loads
        }

        [HttpPost]
        public IActionResult Vote(int selectedCandidateId)
        {
            // Get the list of candidates
            var candidates = _context.Candidates.ToList();

            // Check if the selected candidate exists in the database
            var selectedCandidate = _context.Candidates.FirstOrDefault(c => c.Id == selectedCandidateId);

            if (selectedCandidate != null)
            {
                // Save the vote to the database
                var vote = new Vote { CandidateId = selectedCandidateId };
                _context.Votes.Add(vote);
                _context.SaveChanges();

                // Return a simple thank you message without looking for a view

                return RedirectToAction("ThankYou");


            }
            else
            {
                // If no candidate was found, show an error and pass the candidates list
                ModelState.AddModelError("", "The selected candidate does not exist.");
                return View(candidates);  // Pass candidates to the view again in case of error
            }
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



        // Thank you page after voting
        public IActionResult ThankYou()
        {
            return View();
        }

        // GET: /Voting/Manage
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

        // POST: Voting/Edit/5
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
        // POST: /Voting/Add
        [HttpPost]
        public IActionResult Add(string name)
        {
            // Ensure you're adding the new candidate to the database
            _context.Candidates.Add(new Candidate { Name = name });
            _context.SaveChanges();

            // Redirect or return the updated list of candidates
            return RedirectToAction("Manage");
        }



        // GET: /Voting/Remove
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
