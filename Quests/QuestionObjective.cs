using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Visible through the UI. Quest Objectives allow the player to track completion of a quest.
/// </summary>
public class QuestionObjective
{
    public enum Status { 
        STARTED = 1,
        INPROGRESS = 2,
        COMPLETE = 3,
        FAILED = 4
    }

    public readonly int id;
    public string name;
    public readonly string description;
    private Status status;
    public bool isOptional;
    public readonly int position;

    public QuestionObjective(int id, string name, string description, Status status, bool isOptional, int position)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.status = status;
        this.isOptional = isOptional;
        this.position = position;
    }

    public Status GetStatus()
    {
        return this.status;
    }

    public void SetStatus(Status status)
    {
        this.status = status;
    }

}
