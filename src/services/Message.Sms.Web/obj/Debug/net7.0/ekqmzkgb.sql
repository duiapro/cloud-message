﻿CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231201065937_init') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231201065937_init') THEN

    CREATE TABLE `apiAuthority` (
        `KeyId` char(36) COLLATE ascii_general_ci NOT NULL,
        `Account` longtext CHARACTER SET utf8mb4 NOT NULL,
        `PassWord` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Authority` longtext CHARACTER SET utf8mb4 NOT NULL,
        `CreateTime` datetime(6) NOT NULL,
        `UpdateTime` datetime(6) NOT NULL,
        CONSTRAINT `PK_apiAuthority` PRIMARY KEY (`KeyId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231201065937_init') THEN

    CREATE TABLE `channel` (
        `KeyId` char(36) COLLATE ascii_general_ci NOT NULL,
        `Code` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `Name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `Price` decimal(65,30) NOT NULL,
        `IsActive` tinyint(1) NOT NULL,
        `Description` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
        `CreateTime` datetime(6) NOT NULL,
        `UpdateTime` datetime(6) NOT NULL,
        CONSTRAINT `PK_channel` PRIMARY KEY (`KeyId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231201065937_init') THEN

    CREATE TABLE `users` (
        `KeyId` char(36) COLLATE ascii_general_ci NOT NULL,
        `UserName` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `UserMobile` varchar(15) CHARACTER SET utf8mb4 NOT NULL,
        `PassWork` varchar(15) CHARACTER SET utf8mb4 NOT NULL,
        `Balance` decimal(65,30) NOT NULL,
        `IsVip` tinyint(1) NOT NULL,
        `Discount` decimal(65,30) NOT NULL,
        `IsAdmin` tinyint(1) NOT NULL,
        `CreateTime` datetime(6) NOT NULL,
        `UpdateTime` datetime(6) NOT NULL,
        CONSTRAINT `PK_users` PRIMARY KEY (`KeyId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231201065937_init') THEN

    CREATE TABLE `users_usemobile_history` (
        `KeyId` char(36) COLLATE ascii_general_ci NOT NULL,
        `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
        `ChannelId` longtext CHARACTER SET utf8mb4 NOT NULL,
        `ChannelName` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Mobile` varchar(15) CHARACTER SET utf8mb4 NOT NULL,
        `CreateTime` datetime(6) NOT NULL,
        `UpdateTime` datetime(6) NOT NULL,
        CONSTRAINT `PK_users_usemobile_history` PRIMARY KEY (`KeyId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231201065937_init') THEN

    CREATE TABLE `users_usemobile_codelogs` (
        `KeyId` char(36) COLLATE ascii_general_ci NOT NULL,
        `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
        `ChannelId` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Mobile` varchar(15) CHARACTER SET utf8mb4 NOT NULL,
        `Code` varchar(15) CHARACTER SET utf8mb4 NOT NULL,
        `Context` longtext CHARACTER SET utf8mb4 NOT NULL,
        `UsersKeyId` char(36) COLLATE ascii_general_ci NOT NULL,
        `CreateTime` datetime(6) NOT NULL,
        `UpdateTime` datetime(6) NOT NULL,
        CONSTRAINT `PK_users_usemobile_codelogs` PRIMARY KEY (`KeyId`),
        CONSTRAINT `FK_users_usemobile_codelogs_users_UsersKeyId` FOREIGN KEY (`UsersKeyId`) REFERENCES `users` (`KeyId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231201065937_init') THEN

    CREATE INDEX `IX_users_usemobile_codelogs_UsersKeyId` ON `users_usemobile_codelogs` (`UsersKeyId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231201065937_init') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20231201065937_init', '7.0.2');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231213052138_init2') THEN

    DROP TABLE `apiAuthority`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231213052138_init2') THEN

    ALTER TABLE `users_usemobile_history` MODIFY COLUMN `ChannelId` char(36) COLLATE ascii_general_ci NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231213052138_init2') THEN

    ALTER TABLE `users_usemobile_codelogs` MODIFY COLUMN `ChannelId` char(36) COLLATE ascii_general_ci NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231213052138_init2') THEN

    ALTER TABLE `channel` ADD `ApiServiceProviderId` char(36) COLLATE ascii_general_ci NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231213052138_init2') THEN

    ALTER TABLE `channel` ADD `CostPrice` decimal(65,30) NOT NULL DEFAULT 0.0;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231213052138_init2') THEN

    ALTER TABLE `channel` ADD `Icon` varchar(100) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231213052138_init2') THEN

    CREATE TABLE `api_serviceprovider` (
        `KeyId` char(36) COLLATE ascii_general_ci NOT NULL,
        `Type` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Account` longtext CHARACTER SET utf8mb4 NOT NULL,
        `PassWord` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Authority` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Remark` longtext CHARACTER SET utf8mb4 NOT NULL,
        `CreateTime` datetime(6) NOT NULL,
        `UpdateTime` datetime(6) NOT NULL,
        CONSTRAINT `PK_api_serviceprovider` PRIMARY KEY (`KeyId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231213052138_init2') THEN

    CREATE INDEX `IX_channel_ApiServiceProviderId` ON `channel` (`ApiServiceProviderId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231213052138_init2') THEN

    ALTER TABLE `channel` ADD CONSTRAINT `FK_channel_api_serviceprovider_ApiServiceProviderId` FOREIGN KEY (`ApiServiceProviderId`) REFERENCES `api_serviceprovider` (`KeyId`) ON DELETE CASCADE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231213052138_init2') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20231213052138_init2', '7.0.2');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

