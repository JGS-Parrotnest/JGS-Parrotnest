-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 05, 2026 at 06:26 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `parrotnest`
--

-- --------------------------------------------------------

--
-- Table structure for table `adminactionlog`
--

CREATE TABLE `adminactionlog` (
  `Id` int(11) NOT NULL,
  `PerformedByUserId` int(11) DEFAULT NULL,
  `TargetUserId` int(11) DEFAULT NULL,
  `ActionType` varchar(50) DEFAULT NULL,
  `Reason` varchar(255) DEFAULT NULL,
  `DurationMinutes` varchar(16) DEFAULT NULL,
  `Timestamp` varchar(27) DEFAULT NULL,
  `Details` varchar(512) DEFAULT NULL,
  `Success` tinyint(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `adminactionlog`
--

INSERT INTO `adminactionlog` (`Id`, `PerformedByUserId`, `TargetUserId`, `ActionType`, `Reason`, `DurationMinutes`, `Timestamp`, `Details`, `Success`) VALUES
(1, 1, 7, 'mute', 'Panel admin (bulk)', '60', '2026-02-19 18:01:15.3109257', 'until=2026-02-19T19:01:15.3041906Z', 1),
(2, 1, 7, 'ban', 'Panel admin (bulk)', '', '2026-02-19 18:01:22.9231569', 'until=3026-02-19T18:01:22.9107584Z', 1),
(3, 1, 7, 'unban', '', '', '2026-02-19 18:01:55.2793951', '', 1),
(4, 1, 7, 'mute', 'Panel admin (bulk)', '60', '2026-02-19 18:02:02.698875', 'until=2026-02-19T19:02:02.6862926Z', 1),
(5, 1, 4, 'mute', 'Panel admin (bulk)', '60', '2026-02-19 18:02:02.7165174', 'until=2026-02-19T19:02:02.7155429Z', 1),
(6, 1, 7, 'unmute', '', '', '2026-02-19 18:02:05.0549944', '', 1),
(7, 1, 4, 'unmute', '', '', '2026-02-19 18:02:05.0719555', '', 1),
(8, 1, 7, 'mute', 'dupa', '5', '2026-02-19 18:04:04.3751419', 'until=2026-02-19T18:09:04.3739496Z', 1),
(9, 1, 7, 'ban', 'Panel admin (bulk)', '', '2026-02-19 18:05:49.6631479', 'until=3026-02-19T18:05:49.6615374Z', 1),
(10, 1, 7, 'unban', '', '', '2026-02-19 18:27:05.5852158', '', 1),
(11, 1, 7, 'unmute', '', '', '2026-02-19 18:55:10.208733', '', 1);

-- --------------------------------------------------------

--
-- Table structure for table `adminactionlogs`
--

CREATE TABLE `adminactionlogs` (
  `Id` int(11) NOT NULL,
  `PerformedByUserId` int(11) DEFAULT NULL,
  `TargetUserId` int(11) DEFAULT NULL,
  `ActionType` varchar(50) DEFAULT NULL,
  `Reason` varchar(255) DEFAULT NULL,
  `DurationMinutes` varchar(16) DEFAULT NULL,
  `Timestamp` varchar(27) DEFAULT NULL,
  `Details` varchar(512) DEFAULT NULL,
  `Success` tinyint(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `adminactionlogs`
--

INSERT INTO `adminactionlogs` (`Id`, `PerformedByUserId`, `TargetUserId`, `ActionType`, `Reason`, `DurationMinutes`, `Timestamp`, `Details`, `Success`) VALUES
(1, 1, 7, 'mute', 'Panel admin (bulk)', '60', '2026-02-19 18:01:15.3109257', 'until=2026-02-19T19:01:15.3041906Z', 1),
(2, 1, 7, 'ban', 'Panel admin (bulk)', '', '2026-02-19 18:01:22.9231569', 'until=3026-02-19T18:01:22.9107584Z', 1),
(3, 1, 7, 'unban', '', '', '2026-02-19 18:01:55.2793951', '', 1),
(4, 1, 7, 'mute', 'Panel admin (bulk)', '60', '2026-02-19 18:02:02.698875', 'until=2026-02-19T19:02:02.6862926Z', 1),
(5, 1, 4, 'mute', 'Panel admin (bulk)', '60', '2026-02-19 18:02:02.7165174', 'until=2026-02-19T19:02:02.7155429Z', 1),
(6, 1, 7, 'unmute', '', '', '2026-02-19 18:02:05.0549944', '', 1),
(7, 1, 4, 'unmute', '', '', '2026-02-19 18:02:05.0719555', '', 1),
(8, 1, 7, 'mute', 'dupa', '5', '2026-02-19 18:04:04.3751419', 'until=2026-02-19T18:09:04.3739496Z', 1),
(9, 1, 7, 'ban', 'Panel admin (bulk)', '', '2026-02-19 18:05:49.6631479', 'until=3026-02-19T18:05:49.6615374Z', 1),
(10, 1, 7, 'unban', '', '', '2026-02-19 18:27:05.5852158', '', 1),
(11, 1, 7, 'unmute', '', '', '2026-02-19 18:55:10.208733', '', 1);

-- --------------------------------------------------------

--
-- Table structure for table `friendships`
--

CREATE TABLE `friendships` (
  `Id` int(11) NOT NULL,
  `RequesterId` int(11) NOT NULL,
  `AddresseeId` int(11) NOT NULL,
  `Status` enum('Pending','Accepted','Blocked') DEFAULT 'Pending',
  `CreatedAt` varchar(27) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `friendships`
--

INSERT INTO `friendships` (`Id`, `RequesterId`, `AddresseeId`, `Status`, `CreatedAt`) VALUES
(2, 2, 1, 'Accepted', '2026-02-14 19:09:00.006633'),
(3, 4, 2, 'Accepted', '2026-02-14 19:19:35.9140974'),
(4, 1, 4, 'Accepted', '2026-02-14 19:20:53.2613183');

-- --------------------------------------------------------

--
-- Table structure for table `generalchannelsettings`
--

CREATE TABLE `generalchannelsettings` (
  `Id` int(11) NOT NULL,
  `OwnerId` int(11) DEFAULT NULL,
  `Name` varchar(100) NOT NULL,
  `AvatarUrl` varchar(500) DEFAULT NULL,
  `UpdatedAt` varchar(27) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `generalchannelsettings`
--

INSERT INTO `generalchannelsettings` (`Id`, `OwnerId`, `Name`, `AvatarUrl`, `UpdatedAt`) VALUES
(1, 1, 'Ogólny', '', '2026-02-14 18:10:20.3797733');

-- --------------------------------------------------------

--
-- Table structure for table `groupmembers`
--

CREATE TABLE `groupmembers` (
  `Id` int(11) NOT NULL,
  `GroupId` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `JoinedAt` varchar(27) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `groupmembers`
--

INSERT INTO `groupmembers` (`Id`, `GroupId`, `UserId`, `JoinedAt`) VALUES
(1, 1, 2, '2026-02-14 19:18:45.3544045'),
(2, 1, 4, '2026-02-14 19:18:45.3610331');

-- --------------------------------------------------------

--
-- Table structure for table `groups`
--

CREATE TABLE `groups` (
  `Id` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `OwnerId` int(11) NOT NULL,
  `AvatarUrl` varchar(500) DEFAULT NULL,
  `CreatedAt` varchar(27) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `groups`
--

INSERT INTO `groups` (`Id`, `Name`, `OwnerId`, `AvatarUrl`, `CreatedAt`) VALUES
(1, 'Sey gex', 2, '/uploads/avatars/7acd8588-394f-47aa-ace5-a2b6f4b92a2f.png', '2026-02-14 19:18:45.337928'),
(2, 'pussy', 2, '', '2026-02-22 23:20:55'),
(3, 'pussy', 2, '', '2026-02-22 23:23:32'),
(4, 'pussy', 2, '', '2026-02-22 23:23:44'),
(5, 'pussy', 2, '', '2026-02-22 23:28:22');

-- --------------------------------------------------------

--
-- Table structure for table `messages`
--

CREATE TABLE `messages` (
  `Id` int(11) NOT NULL,
  `SenderId` int(11) NOT NULL,
  `ReceiverId` varchar(16) DEFAULT NULL,
  `GroupId` varchar(16) DEFAULT NULL,
  `Content` text DEFAULT NULL,
  `ImageUrl` varchar(500) DEFAULT NULL,
  `Timestamp` varchar(27) DEFAULT NULL,
  `ReplyToId` varchar(16) DEFAULT NULL,
  `Reactions` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `messages`
--

INSERT INTO `messages` (`Id`, `SenderId`, `ReceiverId`, `GroupId`, `Content`, `ImageUrl`, `Timestamp`, `ReplyToId`, `Reactions`) VALUES
(3, 1, '', '', 'hey admin here', '', '2026-02-14 17:34:02.5930793', '', ''),
(4, 2, '', '', 'hey adddminn', '', '2026-02-14 17:34:26.4448382', '', ''),
(5, 2, '', '', 'halo', '', '2026-02-14 19:03:31.335961', '', ''),
(6, 1, '', '', 'hej', '', '2026-02-14 19:05:05.2741276', '', ''),
(7, 1, '2', '', 'heja', '', '2026-02-14 19:09:11.694831', '', ''),
(8, 2, '1', '', 'no hej', '', '2026-02-14 19:09:16.8954284', '', ''),
(9, 1, '2', '', 'fajne profilowe ziomek', '', '2026-02-14 19:09:39.5865874', '', ''),
(10, 2, '1', '', 'dzięki koleś', '', '2026-02-14 19:09:46.5348674', '', ''),
(11, 4, '', '', 'cześć', '', '2026-02-14 19:11:34.6765083', '', ''),
(12, 4, '', '', '', '/uploads/73c43b86-7ef5-42ad-82ba-0a9d3b4c6280.gif', '2026-02-14 19:16:17.126729', '', ''),
(13, 2, '', '1', 'heja', '', '2026-02-14 19:19:08.9516439', '', ''),
(14, 2, '', '1', 'hej', '', '2026-02-14 19:19:12.7562905', '', ''),
(15, 2, '', '1', 'hejka', '', '2026-02-14 19:19:22.5022516', '', ''),
(16, 2, '', '1', 'hej', '', '2026-02-14 19:19:41.7785252', '', ''),
(17, 2, '', '', '', '/uploads/fe229a48-8edb-46c8-b017-c75a6c0bb11d.jpg', '2026-02-14 19:20:03.1678238', '', '[{\"u\":1,\"e\":\"\\uD83D\\uDE01\"},{\"u\":4,\"e\":\"\\uD83D\\uDE01\"}]'),
(18, 4, '2', '', 'hej', '', '2026-02-14 19:22:09.5531227', '', '[{\"u\":4,\"e\":\"\\uD83D\\uDE42\"}]'),
(19, 4, '1', '', 'no heja', '', '2026-02-14 19:22:23.8206018', '', '[{\"u\":1,\"e\":\"\\uD83D\\uDE00\"},{\"u\":4,\"e\":\"\\uD83D\\uDE00\"}]'),
(20, 1, '2', '', 'spoko', '', '2026-02-14 19:27:10.9111414', '', ''),
(21, 1, '', '', '', '/uploads/f5a8779a-9648-41a0-9957-e1724fd6362c.gif', '2026-02-14 19:27:31.8372765', '', ''),
(22, 4, '', '', 'hejka', '', '2026-02-14 20:12:22.9048974', '', '[{\"u\":4,\"e\":\"\\uD83D\\uDE3F\"},{\"u\":4,\"e\":\"\\uD83D\\uDE40\"},{\"u\":1,\"e\":\"\\uD83D\\uDE3F\"},{\"u\":1,\"e\":\"\\uD83D\\uDE40\"}]'),
(24, 7, '', '', 'ssd', '', '2026-02-15 13:19:54.2345839', '', ''),
(25, 7, '', '', 'jhgh', '', '2026-02-15 14:15:25.3242389', '', ''),
(26, 7, '', '', 'gsd', '', '2026-02-15 14:22:59.3015118', '', ''),
(27, 1, '', '', 'gfd', '', '2026-02-15 14:23:05.5585048', '', ''),
(28, 1, '', '', 'ghdf', '', '2026-02-15 14:23:06.7898785', '', ''),
(29, 7, '', '', 'm', '', '2026-02-15 14:32:01.6600933', '', ''),
(30, 7, '', '', ';lk', '', '2026-02-15 14:32:12.6288047', '', ''),
(31, 7, '', '', ';lk', '', '2026-02-15 14:34:45.8136885', '', ''),
(32, 1, '', '', 'gsdf', '', '2026-02-16 11:33:10.0106358', '', ''),
(33, 7, '', '', 'fds', '', '2026-02-16 11:47:37.9632977', '', ''),
(34, 1, '', '', 'fasd', '', '2026-02-16 11:47:43.4414138', '', ''),
(35, 1, '', '', 'gfd', '', '2026-02-16 11:47:50.5851412', '', ''),
(36, 1, '', '', 'lhkj', '', '2026-02-16 12:25:56.5281076', '', ''),
(37, 7, '', '', 'bf\'', '', '2026-02-16 12:26:04.0893359', '', ''),
(38, 1, '', '', 'hf', '', '2026-02-16 12:29:26.247339', '', ''),
(39, 1, '', '', 'jgh', '', '2026-02-16 12:29:34.6553032', '', ''),
(40, 7, '', '', 'khj', '', '2026-02-16 12:37:24.4557279', '', ''),
(41, 7, '', '', 'hfggfh', '', '2026-02-16 13:38:32.6678821', '', ''),
(42, 7, '', '', 'lih', '', '2026-02-16 13:38:34.0457517', '', ''),
(43, 1, '', '', 'fads', '', '2026-02-16 13:43:01.7773553', '', ''),
(44, 7, '', '', 'hfg', '', '2026-02-16 13:47:51.2217065', '', ''),
(45, 7, '', '', 'fgds', '', '2026-02-16 14:29:29.8391258', '', ''),
(46, 7, '', '', 'ghf', '', '2026-02-16 14:35:17.0288261', '', ''),
(47, 7, '', '', 'cv', '', '2026-02-16 14:41:39.7314551', '', ''),
(48, 7, '', '', 'khj', '', '2026-02-16 14:58:42.610766', '', ''),
(49, 7, '', '', 'd', '', '2026-02-16 14:58:44.1252636', '', ''),
(50, 7, '', '', 'sd', '', '2026-02-16 14:59:06.0759971', '', ''),
(51, 7, '', '', 'jk', '', '2026-02-16 15:03:20.0309057', '', ''),
(52, 7, '', '', 'gjh', '', '2026-02-16 15:07:05.0159551', '', ''),
(53, 7, '', '', 'j', '', '2026-02-16 15:17:11.3240736', '', ''),
(54, 7, '', '', 'j', '', '2026-02-16 15:18:56.2306805', '', ''),
(55, 1, '', '', 'xfg', '', '2026-02-16 15:31:38.1350567', '', ''),
(56, 7, '', '', 'ghchbj', '', '2026-02-16 15:31:45.1949611', '', ''),
(57, 7, '', '', 'fgh', '', '2026-02-16 15:43:38.0682729', '', ''),
(58, 7, '', '', 'klj', '', '2026-02-16 15:43:49.6073731', '', ''),
(59, 1, '', '', 'lj', '', '2026-02-18 17:18:33.3023047', '', ''),
(60, 7, '', '', '', '/uploads/84683106-911d-41e4-be6c-6ae9b61a8e52.gif', '2026-02-18 17:28:18.39182', '', ''),
(62, 1, '', '', '', '/uploads/f94b0e19-5ac2-41c1-8c9e-fec32943aeca.png', '2026-02-19 15:56:22.9885536', '', ''),
(63, 1, '', '', 'this cost now $900 btw', '', '2026-02-19 15:56:47.1656781', '', ''),
(65, 1, '', '', '', '/uploads/585276c1-b572-4a81-a978-5c9de4932af6.mp4', '2026-02-19 16:34:42.492947', '', ''),
(66, 1, '', '', 'fds', '', '2026-02-19 18:05:40.8587288', '', '');

-- --------------------------------------------------------

--
-- Table structure for table `productioncontents`
--

CREATE TABLE `productioncontents` (
  `Id` int(11) NOT NULL,
  `Content` text DEFAULT NULL,
  `UpdatedAt` varchar(27) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `Id` int(11) NOT NULL,
  `Username` varchar(50) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `AvatarUrl` varchar(500) DEFAULT NULL,
  `Status` tinyint(4) DEFAULT NULL,
  `Theme` varchar(20) DEFAULT NULL,
  `TextSize` varchar(20) DEFAULT NULL,
  `IsSimpleText` tinyint(1) DEFAULT NULL,
  `IsAdmin` tinyint(1) DEFAULT NULL,
  `BanUntil` varchar(27) DEFAULT NULL,
  `CreatedAt` varchar(27) DEFAULT NULL,
  `MutedUntil` varchar(27) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`Id`, `Username`, `Email`, `PasswordHash`, `AvatarUrl`, `Status`, `Theme`, `TextSize`, `IsSimpleText`, `IsAdmin`, `BanUntil`, `CreatedAt`, `MutedUntil`) VALUES
(1, 'admin', 'admin@zse.pl', '$2a$11$Ss6wfTTAAUv.0RWvVDCGjOGGtd3Oe0VT/nkfX7cpjGU7db/Mx5FCu', '/uploads/avatars/e25f3550-6f57-4ee6-a2be-90066e443835.png', 1, 'neon', 'medium', 1, 1, '', '2026-02-13 23:20:10.2028259', ''),
(2, 'AdamH', 'adamh@o2.pl', '$2a$11$AM6S5jkCXi/Nle88rLTfbu4MsGolImMqQrmQwFWnOZFsb4xcuif5.', '/uploads/avatars/88d51de1-fb3f-4091-9786-e19b266e6f07.jpg', 1, 'neon', 'medium', 0, 0, '', '2026-02-14 17:09:36.0320492', ''),
(4, 'Hnato', 'hnato@o2.pl', '$2a$11$Ysz.riT6HXSIhdtjOYCk5uTnfCvU1S4oqv3PWuNV6HxtQz0/itbcW', '/uploads/avatars/0812c652-667d-4c74-b1cc-631d806b8dda.jpg', 1, 'original', 'medium', 0, 0, '', '2026-02-14 19:11:19.4896725', ''),
(7, 'dupa', 'dupa@dupa.dupa', '$2a$11$I4UZCYsF7fGNEsw0rFClEeZeesstGpV207TNWWB24eMjsSq0qEemq', '', 1, 'original', 'medium', 0, 0, '', '2026-02-15 13:17:56.5336478', ''),
(8, 'nga', 'pasapasa@o2.pl', 'dhsagdhjg2@123', '', 1, 'neon', 'medium', 0, 0, '', '2026-02-22 23:20:37', '');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `adminactionlog`
--
ALTER TABLE `adminactionlog`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `adminactionlogs`
--
ALTER TABLE `adminactionlogs`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `fk_adminlogs_performedby` (`PerformedByUserId`),
  ADD KEY `fk_adminlogs_target` (`TargetUserId`);

--
-- Indexes for table `friendships`
--
ALTER TABLE `friendships`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `unique_friendship` (`RequesterId`,`AddresseeId`),
  ADD KEY `idx_requester` (`RequesterId`),
  ADD KEY `idx_addressee` (`AddresseeId`),
  ADD KEY `idx_status` (`Status`);

--
-- Indexes for table `generalchannelsettings`
--
ALTER TABLE `generalchannelsettings`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `fk_gcs_owner` (`OwnerId`);

--
-- Indexes for table `groupmembers`
--
ALTER TABLE `groupmembers`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `unique_group_user` (`GroupId`,`UserId`),
  ADD KEY `idx_group` (`GroupId`),
  ADD KEY `idx_user` (`UserId`);

--
-- Indexes for table `groups`
--
ALTER TABLE `groups`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `idx_owner` (`OwnerId`),
  ADD KEY `idx_name` (`Name`);

--
-- Indexes for table `messages`
--
ALTER TABLE `messages`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `idx_sender` (`SenderId`);

--
-- Indexes for table `productioncontents`
--
ALTER TABLE `productioncontents`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `uk_users_username` (`Username`),
  ADD UNIQUE KEY `uk_users_email` (`Email`),
  ADD KEY `idx_email` (`Email`),
  ADD KEY `idx_username` (`Username`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `adminactionlog`
--
ALTER TABLE `adminactionlog`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `adminactionlogs`
--
ALTER TABLE `adminactionlogs`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `friendships`
--
ALTER TABLE `friendships`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `generalchannelsettings`
--
ALTER TABLE `generalchannelsettings`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `groupmembers`
--
ALTER TABLE `groupmembers`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `groups`
--
ALTER TABLE `groups`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `messages`
--
ALTER TABLE `messages`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=67;

--
-- AUTO_INCREMENT for table `productioncontents`
--
ALTER TABLE `productioncontents`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `adminactionlogs`
--
ALTER TABLE `adminactionlogs`
  ADD CONSTRAINT `fk_adminlogs_performedby` FOREIGN KEY (`PerformedByUserId`) REFERENCES `users` (`Id`) ON DELETE SET NULL,
  ADD CONSTRAINT `fk_adminlogs_target` FOREIGN KEY (`TargetUserId`) REFERENCES `users` (`Id`) ON DELETE SET NULL;

--
-- Constraints for table `friendships`
--
ALTER TABLE `friendships`
  ADD CONSTRAINT `fk_friendships_addressee` FOREIGN KEY (`AddresseeId`) REFERENCES `users` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_friendships_requester` FOREIGN KEY (`RequesterId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `generalchannelsettings`
--
ALTER TABLE `generalchannelsettings`
  ADD CONSTRAINT `fk_gcs_owner` FOREIGN KEY (`OwnerId`) REFERENCES `users` (`Id`) ON DELETE SET NULL;

--
-- Constraints for table `groupmembers`
--
ALTER TABLE `groupmembers`
  ADD CONSTRAINT `fk_groupmembers_group` FOREIGN KEY (`GroupId`) REFERENCES `groups` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_groupmembers_user` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `groups`
--
ALTER TABLE `groups`
  ADD CONSTRAINT `fk_groups_owner` FOREIGN KEY (`OwnerId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `messages`
--
ALTER TABLE `messages`
  ADD CONSTRAINT `fk_messages_sender` FOREIGN KEY (`SenderId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
