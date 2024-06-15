namespace RockExchange;

public class AnimationController
{
        // Create a singleton instance
        private static AnimationController? _instance;
        
        private AnimationController()
        {
            // Private constructor to prevent instantiation
        }

        public static AnimationController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AnimationController();
                }

                return _instance;
            }
        }
}