using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

/// <summary>
/// Class to create SofaLink and store them in List
/// </summary>
[System.Serializable]
public class SofaLinkArchiver
{
    /// List of main SofaLink stored in this archiver. Create if a link is added
    public List<SofaLink> m_links = null;

    /// List of slave SofaLink stored in this archiver. Create if a link is added
    public List<SofaLink> m_slaveLinks = null;

    /// Method to add a link to be stored with all the info to create it
    public void AddLink(SofaBaseComponent owner, string linkName, string linkPath)
    {
        if (linkName == "slaves")
        {
            if (m_slaveLinks == null) // first time
                m_slaveLinks = new List<SofaLink>();

            m_slaveLinks.Add(new SofaLink(owner, linkName, linkPath));
        }
        else
        {
            if (m_links == null) // first time
                m_links = new List<SofaLink>();

            m_links.Add(new SofaLink(owner, linkName, linkPath));
        }
    }

    /// Method to get the SofaLink given its name
    public SofaLink GetLink(string linkName)
    {
        foreach (SofaLink link in m_links)
        {
            if (link.LinkName == linkName)
                return link;
        }
        return null;
    }
}
