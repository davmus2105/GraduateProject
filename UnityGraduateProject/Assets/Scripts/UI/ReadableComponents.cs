using System.Collections.Generic;
using UnityEngine;

public class NoteComponent
{
    private string notename;
    private string notetext;
    // ---------------- Methods -----------------
    public string GetName()
    {
        return notename;
    }
    public bool SetName(string newname)
    {
        if (!string.IsNullOrWhiteSpace(newname))
        {
            notename = newname;
            return true;
        }
        return false;
    }
    public string GetText()
    {
        return notetext;
    }
    public bool SetText(string newtext)
    {
        if (!string.IsNullOrWhiteSpace(newtext))
        {
            notetext = newtext;
            return true;
        }
        return false;
    }
}