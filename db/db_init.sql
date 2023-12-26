CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

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

CREATE TABLE `recharge_card` (
    `KeyId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Code` char(36) COLLATE ascii_general_ci NOT NULL,
    `Amount` decimal(65,30) NOT NULL,
    `StartTime` datetime(6) NOT NULL,
    `EndTime` datetime(6) NOT NULL,
    `IsActive` tinyint(1) NOT NULL,
    `Remark` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CreateTime` datetime(6) NOT NULL,
    `UpdateTime` datetime(6) NOT NULL,
    CONSTRAINT `PK_recharge_card` PRIMARY KEY (`KeyId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `users` (
    `KeyId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UserName` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `UserMobile` varchar(15) CHARACTER SET utf8mb4 NOT NULL,
    `PassWork` varchar(15) CHARACTER SET utf8mb4 NOT NULL,
    `Balance` decimal(65,30) NOT NULL,
    `IsVip` tinyint(1) NOT NULL,
    `Discount` decimal(18,2) NOT NULL,
    `IsAdmin` tinyint(1) NOT NULL,
    `CreateTime` datetime(6) NOT NULL,
    `UpdateTime` datetime(6) NOT NULL,
    CONSTRAINT `PK_users` PRIMARY KEY (`KeyId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `users_recharge_logs` (
    `KeyId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Code` char(36) COLLATE ascii_general_ci NOT NULL,
    `UsersRechargeKeyId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Amount` decimal(65,30) NOT NULL,
    `BeforeBalance` decimal(65,30) NOT NULL,
    `AfterBalance` decimal(65,30) NOT NULL,
    `CreateTime` datetime(6) NOT NULL,
    `UpdateTime` datetime(6) NOT NULL,
    CONSTRAINT `PK_users_recharge_logs` PRIMARY KEY (`KeyId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `users_usemobile_history` (
    `KeyId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ChannelId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ApiServiceProviderType` varchar(30) CHARACTER SET utf8mb4 NOT NULL,
    `ChannelName` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Mobile` varchar(15) CHARACTER SET utf8mb4 NOT NULL,
    `CreateTime` datetime(6) NOT NULL,
    `UpdateTime` datetime(6) NOT NULL,
    CONSTRAINT `PK_users_usemobile_history` PRIMARY KEY (`KeyId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `channel` (
    `KeyId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ApiServiceProviderId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Code` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `Icon` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Price` decimal(18,2) NOT NULL,
    `CostPrice` decimal(18,2) NOT NULL,
    `IsActive` tinyint(1) NOT NULL,
    `Description` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
    `CreateTime` datetime(6) NOT NULL,
    `UpdateTime` datetime(6) NOT NULL,
    CONSTRAINT `PK_channel` PRIMARY KEY (`KeyId`),
    CONSTRAINT `FK_channel_api_serviceprovider_ApiServiceProviderId` FOREIGN KEY (`ApiServiceProviderId`) REFERENCES `api_serviceprovider` (`KeyId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `users_usemobile_codelogs` (
    `KeyId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ChannelId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ApiServiceProviderType` varchar(30) CHARACTER SET utf8mb4 NOT NULL,
    `Mobile` varchar(15) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(15) CHARACTER SET utf8mb4 NOT NULL,
    `Context` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CreateTime` datetime(6) NOT NULL,
    `UpdateTime` datetime(6) NOT NULL,
    CONSTRAINT `PK_users_usemobile_codelogs` PRIMARY KEY (`KeyId`),
    CONSTRAINT `FK_users_usemobile_codelogs_users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`KeyId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_channel_ApiServiceProviderId` ON `channel` (`ApiServiceProviderId`);

CREATE INDEX `IX_users_usemobile_codelogs_UserId` ON `users_usemobile_codelogs` (`UserId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20231221062526_init', '7.0.2');

COMMIT;

START TRANSACTION;

CREATE TABLE `project` (
    `KeyId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ProjectName` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `SubTitle` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `Sort` int NOT NULL,
    `Grade` int NOT NULL,
    `Icon` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Status` tinyint(1) NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CreateTime` datetime(6) NOT NULL,
    `UpdateTime` datetime(6) NOT NULL,
    CONSTRAINT `PK_project` PRIMARY KEY (`KeyId`)
) CHARACTER SET=utf8mb4;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20231222020923_init2', '7.0.2');

COMMIT;

START TRANSACTION;

DROP TABLE `users_recharge_logs`;

ALTER TABLE `project` ADD `ApiServiceProviderId` char(36) COLLATE ascii_general_ci NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

ALTER TABLE `project` ADD `ApiServiceProviderType` varchar(30) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

CREATE TABLE `users_balance_bill` (
    `KeyId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UserKeyId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Title` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Type` int NOT NULL,
    `Amount` decimal(65,30) NOT NULL,
    `BeforeBalance` decimal(65,30) NOT NULL,
    `AfterBalance` decimal(65,30) NOT NULL,
    `Remake` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CreateTime` datetime(6) NOT NULL,
    `UpdateTime` datetime(6) NOT NULL,
    CONSTRAINT `PK_users_balance_bill` PRIMARY KEY (`KeyId`),
    CONSTRAINT `FK_users_balance_bill_users_UserKeyId` FOREIGN KEY (`UserKeyId`) REFERENCES `users` (`KeyId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_users_balance_bill_UserKeyId` ON `users_balance_bill` (`UserKeyId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20231225075230_init3', '7.0.2');

COMMIT;

