using UnityEngine.Playables;

namespace Bitfrost.Gameplay.Enemies
{

    public class EnemySpawnerBehaviour : PlayableBehaviour
    {
        private EnemyManager m_manager;
        public bool m_done = false;

        public EnemySpawnConfig spawnConfig;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (!m_done)
            {
                m_manager = playerData as EnemyManager;
                spawnConfig.duration = (float)playable.GetDuration();
                m_manager.ChangeEnemySpawnConfig(spawnConfig);

                m_done = true;
            }
        }

    }
}
