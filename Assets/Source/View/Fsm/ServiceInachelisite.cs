namespace Model
{
    public static class ServiceInachelisite
    {
        public static void RegisterService(MouseDate date, BulletViewFactory bulletFactroy, EnemyViewFactory enemyFactory,
            GainViewFactory gainFactory, WeaponViewFactory weaponFactory,
            ZoneViewFactory zoneFactory, ServiceLocator locator, MapBounds
            bounds, AllGainConffig allGainConffig, AllSoldierConffig allSoldierConffig,
            AllWeaponGameConffig allWeaponGameConffig, PlayerConffig playerConffig, EnemySpawnreConffig enemySpawnreConffig
            , MapConffig mapConffig, BonusSpawnerConffig bonusSpawnerConffig)
        {
            RegisterFactory(locator,bulletFactroy,enemyFactory,gainFactory,weaponFactory,zoneFactory);
            RegisterConffig(locator,allGainConffig,allSoldierConffig,allWeaponGameConffig,playerConffig,bonusSpawnerConffig,enemySpawnreConffig,mapConffig);
            RegisterDate(locator,date);
            RegisterMapBounds(locator,bounds);
            RegisterInput(locator);
        }

        #region RegisterMethod

        public static void RegisterPhysicsRouter(ServiceLocator locator,CollisionRecords records)
        => locator.RegisterService(new PhysicsRouter(records.Values));

        private static void RegisterDate(ServiceLocator serviceLocator,MouseDate mouseDate)
        {
            RegisterMouseDate(serviceLocator,mouseDate);
            RegisterPlayerDate(serviceLocator);
        }

        private static void RegisterFactory(ServiceLocator serviceLocator,BulletViewFactory bulletViewFactory,EnemyViewFactory enemyViewFactory
            ,GainViewFactory gainViewFactory,WeaponViewFactory weaponViewFactory,ZoneViewFactory zoneViewFactory)
        {
            RegisterBulletFactory(serviceLocator,bulletViewFactory);
            RegisterEnemyFactory(serviceLocator,enemyViewFactory);
            RegisterGainFactory(serviceLocator,gainViewFactory);
            RegisterWeaponFactory(serviceLocator,weaponViewFactory);
            RegisterZoneFactroy(serviceLocator,zoneViewFactory);
        }

        private static void RegisterConffig(ServiceLocator serviceLocator,AllGainConffig allGaimConffig, AllSoldierConffig allSoldireConffig,
            AllWeaponGameConffig allWeaponGameConffig,PlayerConffig playerConffig,BonusSpawnerConffig
            bonusSpawnerConffig,EnemySpawnreConffig enemySpawnreConffig,MapConffig mapConffig)
        {
            RegisterAllGainConffig(serviceLocator,allGaimConffig);
            RegisterAllSoldierConffig(serviceLocator, allSoldireConffig);
            RegisterAllWeaponGameConffig( serviceLocator, allWeaponGameConffig);
            RegisterPlayerConffig(serviceLocator, playerConffig);
            RegisterBonusSpawnerConffig(serviceLocator, bonusSpawnerConffig);
            RegisterEnemySpawnerConffig(serviceLocator,enemySpawnreConffig);
            RegisterMapConffig(serviceLocator, mapConffig);
        }

        private static void RegisterMapConffig(ServiceLocator serviceLocator,MapConffig mapConffig)
        => serviceLocator.RegisterService(mapConffig);

        private static void RegisterEnemySpawnerConffig(ServiceLocator serviceLocator, EnemySpawnreConffig enemySpawnreConffig)
        => serviceLocator.RegisterService(enemySpawnreConffig);

        private static void RegisterBonusSpawnerConffig(ServiceLocator serviceLocator,BonusSpawnerConffig bonusSpawnerConffig)
        => serviceLocator.RegisterService(bonusSpawnerConffig);

        private static void RegisterPlayerConffig(ServiceLocator serviceLocator, PlayerConffig playerConffig)
        => serviceLocator.RegisterService(playerConffig);

        private static void RegisterAllWeaponGameConffig(ServiceLocator serviceLocator,AllWeaponGameConffig allWeaponGameConffig)
        => serviceLocator.RegisterService(allWeaponGameConffig);

        private static void RegisterAllSoldierConffig(ServiceLocator serviceLocator,AllSoldierConffig allSoldireConffig)
        => serviceLocator.RegisterService(allSoldireConffig);

        private static void RegisterAllGainConffig(ServiceLocator serviceLocator, AllGainConffig allGaimConffig)
        => serviceLocator.RegisterService(allGaimConffig);

        private static void RegisterMouseDate(ServiceLocator serviceLocator,MouseDate mouseDate)
        => serviceLocator.RegisterService(mouseDate);

        private static void RegisterBulletFactory(ServiceLocator serviceLocator,BulletViewFactory bulletFactory)
        => serviceLocator.RegisterService(bulletFactory);

        private static void RegisterEnemyFactory(ServiceLocator serviceLocator,EnemyViewFactory enemyFactory)
        => serviceLocator.RegisterService(enemyFactory);

        private static void RegisterGainFactory(ServiceLocator serviceLocator,GainViewFactory gainFactory)
        => serviceLocator.RegisterService(gainFactory);

        private static void RegisterWeaponFactory(ServiceLocator serviceLocator,WeaponViewFactory weaponFactory)
        => serviceLocator.RegisterService(weaponFactory);

        private static void RegisterZoneFactroy(ServiceLocator serviceLocator,ZoneViewFactory zoneFactory)
        => serviceLocator.RegisterService(zoneFactory);

        private static void RegisterMapBounds(ServiceLocator serviceLocator,MapBounds mapBounds)
        => serviceLocator.RegisterService(mapBounds);

        private static void RegisterInput(ServiceLocator serviceLocator)
        => serviceLocator.RegisterService(new InputRouter());

        private static void RegisterPlayerDate(ServiceLocator serviceLocator)
        => serviceLocator.RegisterService(new PlayerDate());

        #endregion
    }
}
