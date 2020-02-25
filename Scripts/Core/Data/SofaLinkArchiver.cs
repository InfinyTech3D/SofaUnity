using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

[System.Serializable]
public class SofaLinkArchiver
{
    public List<SofaLink> m_links = null;
    public List<SofaLink> m_slaveLinks = null;

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
