namespace Notes.StateContainers
{
    public class DarkModeSwitchStateContainer
    {
        private bool IsDarkModeOn { get; set; }

        public Func<Task>? OnDarkModeChanged { get; set; }

        public async Task SetDarkMode(bool isDarkModeOn)
        {
            IsDarkModeOn = isDarkModeOn;

            if (OnDarkModeChanged != null)
            {
                await OnDarkModeChanged.Invoke();
            }
        }

        public bool GetDarkMode()
        {
            return IsDarkModeOn;
        }
    }
}
