public interface IPausable
{
	public bool isPaused { get; set; }

	public void Pause();
	public void Resume();

}

/*

	// Basic Interface Implement

    public bool isPaused { get; set; }

	#region Pause


    public void Pause()
    {
        isPaused = true;

    }

    public void Resume()
    {
        isPaused = false;
    }

    #endregion

	
	// With event subscription

	if (m_levelChannel != null)
	{
		m_levelChannel.onPauseGame += Pause;
		m_levelChannel.onResumeGame += Resume;

	}

		if (m_levelChannel != null)
	{
		m_levelChannel.onPauseGame -= Pause;
		m_levelChannel.onResumeGame -= Resume;

	}

*/