﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OperationCHAN.Areas.Lists.Pages;
using OperationCHAN.Data;
using OperationCHAN.Models;

namespace OperationCHAN.Areas.Ticket.Pages;

public class Queue : PageModel
{
    private readonly ApplicationDbContext _db;
    
    public IEnumerable<int> HelpLists { get; set; }
    public IQueryable<HelplistModel> Ticket { get; set; }
    private IQueryable<HelplistModel> Tickets { get; set; }
    
    public int Count { get; set; }
    
    public Queue(ApplicationDbContext db)
    {
        _db = db;
        HelpLists = _db.HelpList.Select(helplist => helplist.Id).Distinct();
    }

    public IActionResult OnGet(int id)
    {
       
        if (!HelpLists.Contains(id))
        {
            return Redirect("/404");
        }

        var ticket = _db.HelpList.Where(ticket => ticket.Id == id);
        var tickets = _db.HelpList.Where(t => t.Course == ticket.First().Course && t.Status == "Waiting");

        Ticket = ticket;
        Tickets = tickets;
        
        if (Ticket.First().Status == "Removed" || Ticket.First().Status == "Finished")
        {
            return Redirect("/identity/error");
        }

        var count = 0;
        foreach (var t in tickets)
        {
            if (t.Id == ticket.First().Id)
            {
                break;
            }

            count++;
        }


        Count = count;
        
        return Page();
    }
}