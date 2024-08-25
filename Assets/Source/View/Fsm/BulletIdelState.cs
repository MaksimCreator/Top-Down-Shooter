namespace View
{
    public class BulletIdelState : FsmState
    {
        private readonly IBulletData _bulletData;

        public BulletIdelState(IBulletData data,Fsm fsm) : base(fsm)
        {
            _bulletData = data;
        }

        public override void Update()
        {
            if (_bulletData.isMoveming)
                Fsm.SetState<BulletMovemengState>();
        }
    }
}
