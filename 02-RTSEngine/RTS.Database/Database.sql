-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- Hôte : localhost
-- Généré le :  mar. 12 nov. 2019 à 20:14
-- Version du serveur :  8.0.13-4
-- Version de PHP :  7.2.24-0ubuntu0.18.04.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données :  `pAa2bIMH5M`
--

-- --------------------------------------------------------

--
-- Structure de la table `building`
--

CREATE TABLE `building` (
  `id` int(11) NOT NULL,
  `name` varchar(48) COLLATE utf8_unicode_ci NOT NULL,
  `building_type_id` int(11) NOT NULL DEFAULT '0',
  `resource_cost_id` int(11) NOT NULL,
  `construction_time` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `building`
--

INSERT INTO `building` (`id`, `name`, `building_type_id`, `resource_cost_id`, `construction_time`) VALUES
(1, 'Brocco farm', 2, 1, 15),
(2, 'Neo grav', 2, 4, 15),
(3, 'Mega mine', 2, 7, 15),
(4, 'AniFood factory', 2, 10, 60),
(5, 'Conserve company', 2, 13, 120),
(6, 'M3D company', 2, 16, 300),
(7, 'RBMK3', 3, 19, 300),
(8, 'Command center', 1, 0, 0),
(9, 'Motel', 3, 21, 21600);

-- --------------------------------------------------------

--
-- Structure de la table `building_center`
--

CREATE TABLE `building_center` (
  `building_id` int(11) NOT NULL COMMENT 'id of the building'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `building_center`
--

INSERT INTO `building_center` (`building_id`) VALUES
(8);

-- --------------------------------------------------------

--
-- Structure de la table `building_center_level`
--

CREATE TABLE `building_center_level` (
  `building_id` int(11) NOT NULL,
  `level_id` int(11) NOT NULL,
  `persistent_resource_bag_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `building_center_level`
--

INSERT INTO `building_center_level` (`building_id`, `level_id`, `persistent_resource_bag_id`) VALUES
(8, 1, 28);

-- --------------------------------------------------------

--
-- Structure de la table `building_level_cost`
--

CREATE TABLE `building_level_cost` (
  `building_id` int(11) NOT NULL,
  `level_id` int(11) NOT NULL,
  `resource_cost_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `building_level_cost`
--

INSERT INTO `building_level_cost` (`building_id`, `level_id`, `resource_cost_id`) VALUES
(1, 1, 0),
(1, 2, 2),
(1, 3, 3),
(2, 1, 0),
(2, 2, 5),
(2, 3, 6),
(3, 1, 0),
(3, 2, 8),
(3, 3, 9),
(4, 1, 0),
(4, 2, 11),
(4, 3, 12),
(5, 1, 0),
(5, 2, 14),
(5, 3, 15),
(6, 1, 0),
(6, 2, 17),
(6, 3, 18),
(7, 1, 0),
(7, 2, 20),
(8, 1, 0),
(9, 1, 0);

-- --------------------------------------------------------

--
-- Structure de la table `building_passive`
--

CREATE TABLE `building_passive` (
  `building_id` int(11) NOT NULL COMMENT 'ID of the building'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `building_passive`
--

INSERT INTO `building_passive` (`building_id`) VALUES
(7),
(9);

-- --------------------------------------------------------

--
-- Structure de la table `building_passive_level`
--

CREATE TABLE `building_passive_level` (
  `building_id` int(11) NOT NULL,
  `level_id` int(11) NOT NULL,
  `persistent_resource_bag_id_needed` int(11) NOT NULL DEFAULT '0' COMMENT 'Persistent bag needed to work correctly',
  `persistent_resource_bag_id` int(11) NOT NULL DEFAULT '0',
  `seconds_to_consume` int(11) NOT NULL,
  `consumption_resource_bag_id` int(11) NOT NULL DEFAULT '0' COMMENT 'Resources needed to start a new passive cycle'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `building_passive_level`
--

INSERT INTO `building_passive_level` (`building_id`, `level_id`, `persistent_resource_bag_id_needed`, `persistent_resource_bag_id`, `seconds_to_consume`, `consumption_resource_bag_id`) VALUES
(7, 1, 0, 22, 300, 24),
(7, 2, 0, 23, 300, 25),
(9, 1, 27, 26, 300, 0),
(9, 2, 0, 0, 300, 0);

-- --------------------------------------------------------

--
-- Structure de la table `building_producer`
--

CREATE TABLE `building_producer` (
  `building_id` int(11) NOT NULL,
  `default_resource_id_produced` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT;

--
-- Déchargement des données de la table `building_producer`
--

INSERT INTO `building_producer` (`building_id`, `default_resource_id_produced`) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 6);

-- --------------------------------------------------------

--
-- Structure de la table `building_producer_level`
--

CREATE TABLE `building_producer_level` (
  `building_id` int(11) NOT NULL,
  `resource_id_produced` int(11) NOT NULL,
  `level_id` int(11) NOT NULL,
  `persistent_resource_bag_id_needed` int(11) NOT NULL DEFAULT '0' COMMENT 'Persistent bag needed to work correctly',
  `seconds_to_produce` int(11) NOT NULL,
  `amount_produced` int(11) NOT NULL,
  `consumption_resource_bag_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT;

--
-- Déchargement des données de la table `building_producer_level`
--

INSERT INTO `building_producer_level` (`building_id`, `resource_id_produced`, `level_id`, `persistent_resource_bag_id_needed`, `seconds_to_produce`, `amount_produced`, `consumption_resource_bag_id`) VALUES
(1, 1, 1, 29, 300, 10, 0),
(1, 1, 2, 30, 300, 15, 0),
(1, 1, 3, 31, 300, 20, 0),
(2, 2, 1, 32, 300, 10, 0),
(2, 2, 2, 33, 300, 15, 0),
(2, 2, 3, 34, 300, 20, 0),
(3, 3, 1, 35, 300, 30, 0),
(3, 3, 2, 36, 300, 40, 0),
(3, 3, 3, 37, 300, 50, 0),
(4, 4, 1, 38, 300, 10, 47),
(4, 4, 2, 39, 300, 15, 48),
(4, 4, 3, 40, 300, 20, 49),
(5, 5, 1, 41, 300, 10, 50),
(5, 5, 2, 42, 300, 15, 51),
(5, 5, 3, 43, 300, 20, 52),
(6, 6, 1, 44, 300, 10, 53),
(6, 6, 2, 45, 300, 15, 54),
(6, 6, 3, 46, 300, 20, 55);

-- --------------------------------------------------------

--
-- Structure de la table `building_state`
--

CREATE TABLE `building_state` (
  `id` int(11) NOT NULL,
  `name` varchar(48) COLLATE utf8_unicode_ci NOT NULL,
  `can_produce` int(11) NOT NULL,
  `can_activate` int(11) NOT NULL,
  `can_repair` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `building_state`
--

INSERT INTO `building_state` (`id`, `name`, `can_produce`, `can_activate`, `can_repair`) VALUES
(1, 'Construction', 0, 0, 0),
(2, 'Active', 1, 1, 1),
(3, 'Paused', 0, 1, 1),
(4, 'Out of raw material', 0, 1, 1),
(5, 'Out of energy', 0, 0, 1);

-- --------------------------------------------------------

--
-- Structure de la table `city_level`
--

CREATE TABLE `city_level` (
  `id` int(5) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `city_level`
--

INSERT INTO `city_level` (`id`) VALUES
(1),
(2),
(3);

-- --------------------------------------------------------

--
-- Structure de la table `game_server_zone`
--

CREATE TABLE `game_server_zone` (
  `server_name` varchar(15) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `environment` varchar(20) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `world_zone_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `game_server_zone`
--

INSERT INTO `game_server_zone` (`server_name`, `environment`, `world_zone_id`) VALUES
('GameServer1', 'DEV_DAVID', 1),
('GameServer1', 'DEV_DAVID', 2),
('GameServer1', 'DEV_OLIVIER', 1),
('GameServer1', 'DEV_OLIVIER', 2);

-- --------------------------------------------------------

--
-- Structure de la table `map_element`
--

CREATE TABLE `map_element` (
  `id` int(11) NOT NULL,
  `name` varchar(45) COLLATE utf8_unicode_ci NOT NULL,
  `map_element_type_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `map_element`
--

INSERT INTO `map_element` (`id`, `name`, `map_element_type_id`) VALUES
(1, 'Random', 4),
(2, 'Building spot basic', 1),
(3, 'Resource', 2),
(4, 'Decoration', 3),
(5, 'Player building center', 5);

-- --------------------------------------------------------

--
-- Structure de la table `map_element_type`
--

CREATE TABLE `map_element_type` (
  `id` int(11) NOT NULL,
  `name` varchar(30) COLLATE utf8_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `map_element_type`
--

INSERT INTO `map_element_type` (`id`, `name`) VALUES
(1, 'Building spot'),
(2, 'Resource'),
(3, 'Decoration'),
(4, 'Random'),
(5, 'Player building center');

-- --------------------------------------------------------

--
-- Structure de la table `map_extent`
--

CREATE TABLE `map_extent` (
  `id` int(11) NOT NULL,
  `name` varchar(256) COLLATE utf8_unicode_ci NOT NULL,
  `width` int(11) DEFAULT NULL,
  `depth` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `map_extent`
--

INSERT INTO `map_extent` (`id`, `name`, `width`, `depth`) VALUES
(1, 'Adventure starts here...', 0, 0),
(2, 'Adventure continue', 0, 0);

-- --------------------------------------------------------

--
-- Structure de la table `map_extent_element`
--

CREATE TABLE `map_extent_element` (
  `map_extent_id` int(11) NOT NULL,
  `position` varchar(25) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `map_element_id` int(11) NOT NULL,
  `entity_id` int(11) DEFAULT NULL COMMENT 'ID of the element referenced (can be a building, a resource, ...)',
  `instance_id` int(11) NOT NULL DEFAULT '0' COMMENT 'Id of the map element instance in the map extent'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `map_extent_element`
--

INSERT INTO `map_extent_element` (`map_extent_id`, `position`, `map_element_id`, `entity_id`, `instance_id`) VALUES
(1, '-0.30, 1.77, 0.00', 3, 0, 11),
(1, '-0.75, -0.02, 0.00', 2, 0, 3),
(1, '-1.62, -0.95, 0.00', 3, 0, 8),
(1, '0.47, -0.89, 0.00', 2, 0, 6),
(1, '0.64, 1.54, 0.00', 2, 0, 4),
(1, '1.22, 0.37, 0.00', 5, 0, 1),
(1, '1.67, -1.41, 0.00', 3, 0, 9),
(1, '2.18, 1.43, 0.00', 2, 0, 5),
(1, '3.13, -0.60, 0.00', 2, 0, 7),
(1, '3.30, 0.46, 0.00', 2, 0, 2),
(1, '3.33, 1.79, 0.00', 3, 0, 10),
(2, '-1.60, 2.82, 0.00', 3, 0, 6),
(2, '-1.80, 1.14, 0.00', 2, 0, 2),
(2, '-2.98, 2.27, 0.00', 2, 0, 4),
(2, '-3.06, 0.14, 0.00', 2, 0, 3),
(2, '-4.08, 1.08, 0.00', 2, 0, 1),
(2, '-4.81, 1.79, 0.00', 3, 0, 5);

-- --------------------------------------------------------

--
-- Structure de la table `market_level`
--

CREATE TABLE `market_level` (
  `id` int(11) NOT NULL,
  `amount_receive_limit` int(11) NOT NULL COMMENT 'maximum amount of resource traded'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `market_level`
--

INSERT INTO `market_level` (`id`, `amount_receive_limit`) VALUES
(1, 100);

-- --------------------------------------------------------

--
-- Structure de la table `market_resource_ratio`
--

CREATE TABLE `market_resource_ratio` (
  `resource_id_given` int(11) NOT NULL,
  `resource_id_received` int(11) NOT NULL,
  `amount_received_for_one_given` int(11) NOT NULL COMMENT 'For 1 resource A, we need [amount] resource B'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `market_resource_ratio`
--

INSERT INTO `market_resource_ratio` (`resource_id_given`, `resource_id_received`, `amount_received_for_one_given`) VALUES
(2, 1, 2),
(3, 1, 5),
(3, 2, 2),
(4, 1, 10),
(4, 2, 5),
(4, 3, 2),
(5, 1, 15),
(5, 2, 5),
(5, 3, 3),
(5, 4, 5),
(6, 1, 20),
(6, 2, 15),
(6, 3, 5),
(6, 4, 5),
(6, 5, 2);

-- --------------------------------------------------------

--
-- Structure de la table `player`
--

CREATE TABLE `player` (
  `id` int(11) NOT NULL,
  `device_id` varchar(128) COLLATE utf8_unicode_ci NOT NULL,
  `nickname` varchar(32) CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT NULL,
  `password` varchar(256) CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT NULL,
  `creation` datetime DEFAULT NULL,
  `current_token` varchar(255) CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `player`
--

INSERT INTO `player` (`id`, `device_id`, `nickname`, `password`, `creation`, `current_token`) VALUES
(173, 'dae9678ac18de179cc4bce845ddc36fadbf072d8', 'Olivier', NULL, '2019-10-18 13:48:34', 'TT/K+7+cCkavsP0B08eVWA=='),
(178, '38cca29719dec5cfe9eef0a25ca2fa5e82f88844', 'Fax', NULL, '2019-11-05 21:26:08', ''),
(179, '8695040b81ac26054ec58e3bc234b8698e64027f', 'David', NULL, '2019-11-06 20:47:16', 'fWIz3qnjxUOnJ5v+rt1+0A==');

-- --------------------------------------------------------

--
-- Structure de la table `player_building`
--

CREATE TABLE `player_building` (
  `player_id` int(11) NOT NULL,
  `building_id` int(11) NOT NULL,
  `building_number` int(11) NOT NULL,
  `creation` datetime NOT NULL,
  `state_id` int(11) NOT NULL,
  `level_id` int(11) NOT NULL DEFAULT '1',
  `map_extent_id` int(11) NOT NULL COMMENT 'id of the extent where the building is built',
  `map_element_instance_id` int(11) NOT NULL COMMENT 'id of the instance element where the building is built'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT;

--
-- Déchargement des données de la table `player_building`
--

INSERT INTO `player_building` (`player_id`, `building_id`, `building_number`, `creation`, `state_id`, `level_id`, `map_extent_id`, `map_element_instance_id`) VALUES
(173, 8, 0, '2019-10-18 13:48:34', 2, 1, 1, 1),
(174, 8, 0, '2019-10-18 13:56:32', 2, 1, 1, 1),
(175, 8, 0, '2019-11-05 12:22:12', 2, 1, 1, 1),
(176, 8, 0, '2019-11-05 14:09:31', 2, 1, 1, 1),
(177, 8, 0, '2019-11-05 14:10:45', 2, 1, 1, 1),
(178, 8, 0, '2019-11-05 21:26:09', 2, 1, 1, 1),
(179, 8, 0, '2019-11-06 20:47:17', 2, 1, 1, 1);

-- --------------------------------------------------------

--
-- Structure de la table `player_building_center`
--

CREATE TABLE `player_building_center` (
  `player_id` int(11) NOT NULL,
  `building_id` int(11) NOT NULL,
  `building_number` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `player_building_center`
--

INSERT INTO `player_building_center` (`player_id`, `building_id`, `building_number`) VALUES
(173, 8, 0),
(174, 8, 0),
(175, 8, 0),
(176, 8, 0),
(177, 8, 0),
(178, 8, 0),
(179, 8, 0);

-- --------------------------------------------------------

--
-- Structure de la table `player_building_passive`
--

CREATE TABLE `player_building_passive` (
  `player_id` int(11) NOT NULL COMMENT 'ID of the player',
  `building_id` int(11) NOT NULL COMMENT 'ID of the building',
  `building_number` int(11) NOT NULL COMMENT 'Number ID of the building',
  `start_consumption` datetime DEFAULT NULL,
  `last_consumption` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Structure de la table `player_building_producer`
--

CREATE TABLE `player_building_producer` (
  `player_id` int(11) NOT NULL,
  `building_id` int(11) NOT NULL,
  `building_number` int(11) NOT NULL,
  `auto_produce` int(11) NOT NULL,
  `current_resource_id_produced` int(11) NOT NULL DEFAULT '0',
  `start_production` datetime DEFAULT NULL,
  `last_production` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT;

-- --------------------------------------------------------

--
-- Structure de la table `player_city`
--

CREATE TABLE `player_city` (
  `player_id` int(5) NOT NULL,
  `position` char(10) COLLATE utf8_unicode_ci NOT NULL,
  `level_id` int(5) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Structure de la table `player_map`
--

CREATE TABLE `player_map` (
  `player_id` int(11) NOT NULL,
  `name` varchar(256) COLLATE utf8_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `player_map`
--

INSERT INTO `player_map` (`player_id`, `name`) VALUES
(173, ''),
(174, ''),
(175, ''),
(176, ''),
(177, ''),
(178, ''),
(179, '');

-- --------------------------------------------------------

--
-- Structure de la table `player_map_extent`
--

CREATE TABLE `player_map_extent` (
  `player_id` int(11) NOT NULL,
  `map_extent_id` int(11) NOT NULL,
  `creation` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `player_map_extent`
--

INSERT INTO `player_map_extent` (`player_id`, `map_extent_id`, `creation`) VALUES
(173, 1, '2019-10-18 13:48:34'),
(174, 1, '2019-10-18 13:56:31'),
(175, 1, '2019-11-05 12:22:12'),
(176, 1, '2019-11-05 14:09:30'),
(177, 1, '2019-11-05 14:10:45'),
(178, 1, '2019-11-05 21:26:09'),
(179, 1, '2019-11-06 20:47:16');

-- --------------------------------------------------------

--
-- Structure de la table `player_map_extent_element`
--

CREATE TABLE `player_map_extent_element` (
  `player_id` int(11) NOT NULL,
  `map_extent_id` int(11) NOT NULL,
  `map_element_id` int(11) NOT NULL,
  `entity_id` int(11) NOT NULL,
  `map_element_instance_id` int(11) NOT NULL DEFAULT '0' COMMENT 'Id of the map element instance in the map extent'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `player_map_extent_element`
--

INSERT INTO `player_map_extent_element` (`player_id`, `map_extent_id`, `map_element_id`, `entity_id`, `map_element_instance_id`) VALUES
(173, 1, 3, 2, 11),
(173, 1, 3, 1, 8),
(173, 1, 3, 2, 9),
(173, 1, 3, 1, 10),
(174, 1, 3, 1, 11),
(174, 1, 3, 2, 8),
(174, 1, 3, 3, 9),
(174, 1, 3, 1, 10),
(175, 1, 3, 3, 11),
(175, 1, 3, 3, 8),
(175, 1, 3, 2, 9),
(175, 1, 3, 2, 10),
(176, 1, 3, 3, 11),
(176, 1, 3, 1, 8),
(176, 1, 3, 2, 9),
(176, 1, 3, 3, 10),
(177, 1, 3, 1, 11),
(177, 1, 3, 2, 8),
(177, 1, 3, 2, 9),
(177, 1, 3, 2, 10),
(178, 1, 3, 3, 11),
(178, 1, 3, 1, 8),
(178, 1, 3, 3, 9),
(178, 1, 3, 1, 10),
(179, 1, 3, 1, 11),
(179, 1, 3, 3, 8),
(179, 1, 3, 3, 9),
(179, 1, 3, 1, 10);

-- --------------------------------------------------------

--
-- Structure de la table `player_market`
--

CREATE TABLE `player_market` (
  `player_id` int(11) NOT NULL,
  `level_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `player_market`
--

INSERT INTO `player_market` (`player_id`, `level_id`) VALUES
(173, 1),
(174, 1),
(175, 1),
(176, 1),
(177, 1),
(178, 1),
(179, 1);

-- --------------------------------------------------------

--
-- Structure de la table `player_market_trades`
--

CREATE TABLE `player_market_trades` (
  `player_id` int(11) NOT NULL,
  `resource_id_given` int(11) NOT NULL,
  `resource_id_received` int(11) NOT NULL,
  `quantity` int(11) NOT NULL COMMENT 'This is the quantity of ratio. Enable to calculate amounts',
  `amount_received_for_one_given` int(11) NOT NULL COMMENT 'ratio of the trade',
  `creation` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Structure de la table `player_resource_bag`
--

CREATE TABLE `player_resource_bag` (
  `player_id` int(11) NOT NULL COMMENT 'ID of the player',
  `resource_id` int(11) NOT NULL COMMENT 'ID of the resource',
  `amount` int(11) NOT NULL DEFAULT '0' COMMENT 'Current amount of the bag',
  `maximum` int(11) NOT NULL DEFAULT '0' COMMENT 'Maximum ressources that the bag can handle',
  `used` int(11) NOT NULL DEFAULT '0' COMMENT 'Resources currently used'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `player_resource_bag`
--

INSERT INTO `player_resource_bag` (`player_id`, `resource_id`, `amount`, `maximum`, `used`) VALUES
(173, 1, 0, 3000, 0),
(173, 2, 0, 3000, 0),
(173, 3, 0, 3000, 0),
(173, 4, 0, 3000, 0),
(173, 5, 0, 3000, 0),
(173, 6, 1250, 3000, 0),
(173, 7, 10, -1, 0),
(173, 8, 10, -1, 0),
(174, 1, 0, 3000, 0),
(174, 2, 0, 3000, 0),
(174, 3, 0, 3000, 0),
(174, 4, 0, 3000, 0),
(174, 5, 0, 3000, 0),
(174, 6, 1250, 3000, 0),
(174, 7, 10, -1, 0),
(174, 8, 10, -1, 0),
(175, 1, 0, 3000, 0),
(175, 2, 0, 3000, 0),
(175, 3, 0, 3000, 0),
(175, 4, 0, 3000, 0),
(175, 5, 0, 3000, 0),
(175, 6, 1250, 3000, 0),
(175, 7, 10, -1, 0),
(175, 8, 10, -1, 0),
(176, 1, 0, 3000, 0),
(176, 2, 0, 3000, 0),
(176, 3, 0, 3000, 0),
(176, 4, 0, 3000, 0),
(176, 5, 0, 3000, 0),
(176, 6, 1250, 3000, 0),
(176, 7, 10, -1, 0),
(176, 8, 10, -1, 0),
(177, 1, 0, 3000, 0),
(177, 2, 0, 3000, 0),
(177, 3, 0, 3000, 0),
(177, 4, 0, 3000, 0),
(177, 5, 0, 3000, 0),
(177, 6, 1250, 3000, 0),
(177, 7, 10, -1, 0),
(177, 8, 10, -1, 0),
(178, 1, 0, 3000, 0),
(178, 2, 0, 3000, 0),
(178, 3, 0, 3000, 0),
(178, 4, 0, 3000, 0),
(178, 5, 0, 3000, 0),
(178, 6, 1250, 3000, 0),
(178, 7, 10, -1, 0),
(178, 8, 10, -1, 0),
(179, 1, 0, 3000, 0),
(179, 2, 0, 3000, 0),
(179, 3, 0, 3000, 0),
(179, 4, 0, 3000, 0),
(179, 5, 0, 3000, 0),
(179, 6, 1250, 3000, 0),
(179, 7, 10, -1, 0),
(179, 8, 10, -1, 0);

-- --------------------------------------------------------

--
-- Structure de la table `player_resource_bag_default`
--

CREATE TABLE `player_resource_bag_default` (
  `resource_id` int(11) NOT NULL,
  `amount` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT;

--
-- Déchargement des données de la table `player_resource_bag_default`
--

INSERT INTO `player_resource_bag_default` (`resource_id`, `amount`) VALUES
(1, 0),
(2, 0),
(3, 0),
(4, 0),
(5, 0),
(6, 1250),
(7, 0),
(8, 0);

-- --------------------------------------------------------

--
-- Structure de la table `player_sessions`
--

CREATE TABLE `player_sessions` (
  `player_id` int(11) NOT NULL,
  `token` varchar(255) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `start` date NOT NULL,
  `end` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `player_sessions`
--

INSERT INTO `player_sessions` (`player_id`, `token`, `start`, `end`) VALUES
(179, 'F2g4a1CbckOD1OHajoRFtQ==', '2019-11-08', '0001-01-01'),
(179, 'nnm+93JhJ0iGPdPBEDNhIA==', '2019-11-09', '0001-01-01'),
(179, 'z3pTeeo0sUW2CcOVp+5BSg==', '2019-11-09', '0001-01-01'),
(179, '8Os+LWG+ekqxY0NMVao28A==', '2019-11-09', '0001-01-01'),
(179, 'geWeQbk7m0Svw7sKwCO1Fg==', '2019-11-09', '0001-01-01'),
(179, 'mR5yphB5B0eHGeNFzMyUHg==', '2019-11-09', '0001-01-01'),
(179, 'dSKfzzNrrkqSdllFBAZpRw==', '2019-11-09', '0001-01-01'),
(179, 'YfhtOZpeB06ZxjPBBEIl/Q==', '2019-11-09', '0001-01-01'),
(179, 'dRzctfFC40ysshqjPkNnOw==', '2019-11-09', '0001-01-01'),
(179, 'W8JxT/SgsUypBLwxqBvDBg==', '2019-11-09', '0001-01-01'),
(179, 'D6bRN+SlUkiyvRBoodxLOg==', '2019-11-09', '0001-01-01'),
(179, 'RLTB9hpmN0SOzHNoz37Hgw==', '2019-11-09', '0001-01-01'),
(179, 'oIDww88/cUmIyw+YYny1lQ==', '2019-11-09', '0001-01-01'),
(179, 'mrnzOqdrhkOr7OmG1suqBA==', '2019-11-09', '0001-01-01'),
(179, 'XWJj6dylRky5CSDJGo+MPw==', '2019-11-09', '0001-01-01'),
(179, 'IEkyv0MaVEGm3AhMbb1tNQ==', '2019-11-09', '0001-01-01'),
(179, 'FVw5nbEtCkazSbXoLNBWOA==', '2019-11-09', '0001-01-01'),
(179, 'X8X4/BIfT0ilK922HPhZVQ==', '2019-11-09', '0001-01-01'),
(179, 'nMtWLb2RGk29Llzz49/CMQ==', '2019-11-09', '0001-01-01'),
(179, 'elgeQzb0NkiAQ7lc0XbgDQ==', '2019-11-09', '0001-01-01'),
(179, 'SHTJwGZctkq1CR46gPb06w==', '2019-11-09', '0001-01-01'),
(179, 'Gz+3s5LXi0uo1Natic5AzA==', '2019-11-09', '0001-01-01'),
(179, '//3GyhILtkaNZTAA58RQNA==', '2019-11-09', '0001-01-01'),
(179, 'r030m3XHdUijpMfhFdLlOw==', '2019-11-09', '0001-01-01'),
(179, 'MlfFnMSpH0SpBeZCXO3VvA==', '2019-11-09', '0001-01-01'),
(179, 'GnZawPn/z0GFkOpgZnHBIg==', '2019-11-09', '0001-01-01'),
(179, '4LxH+QRAXEq+K861+WCPYA==', '2019-11-09', '0001-01-01'),
(179, 'xrgV8P6lAEK2aFkHGO9uvA==', '2019-11-09', '0001-01-01'),
(179, 'xt7wQ1XcjE+wWZputRlyZA==', '2019-11-09', '0001-01-01'),
(179, 'BqNNfCGNy0uCsP18Lga2YA==', '2019-11-09', '0001-01-01'),
(179, 'BnTEkJKCmk+eGBFuYDpBcQ==', '2019-11-10', '0001-01-01'),
(179, 'w00AjRV53UepIoACuoheOA==', '2019-11-10', '0001-01-01'),
(179, 'xqIHFtNluka7f6rcVyLwqQ==', '2019-11-10', '0001-01-01'),
(179, '8HnZmgtxOUeH+vGr3p7GDg==', '2019-11-10', '0001-01-01'),
(179, 'yOhr3Bb4sUu4ONXlJE0abQ==', '2019-11-10', '0001-01-01'),
(179, 'YOCYTQ8KyUqmOZva0u9JOA==', '2019-11-10', '0001-01-01'),
(179, 'AiywhCkrCkS8H5VE50QWrQ==', '2019-11-10', '0001-01-01'),
(179, 'w7yuav+XDES/b3T8UIaBHA==', '2019-11-10', '0001-01-01'),
(179, 'BGlPzT3N9EWenoPAgIoLtA==', '2019-11-10', '0001-01-01'),
(179, '949FWoNr5kyCDEjCFz5JFQ==', '2019-11-10', '0001-01-01'),
(179, '5PnNjl6jTk2QvpphL5JxHA==', '2019-11-10', '0001-01-01'),
(179, 'dtgQy/sseUiZtYlo6qRFXA==', '2019-11-10', '0001-01-01'),
(179, '0tRSDJOSCU+6Rs8VUOeiOQ==', '2019-11-10', '0001-01-01'),
(179, 'c0ix1b3pQ0+6Ik6Js6w/XQ==', '2019-11-10', '0001-01-01'),
(179, '6rVerrN1e0CAKbr8jOxWMA==', '2019-11-10', '0001-01-01'),
(179, '1gcl3BgTsk+CtdGBSWC/hA==', '2019-11-10', '0001-01-01'),
(179, 'y8MqmWm/hUC8UgBJ5tf5bw==', '2019-11-10', '0001-01-01'),
(179, 'ciMwqea7ykCGBybBEDQvEA==', '2019-11-10', '0001-01-01'),
(179, 'QgzRSXZ4YUGzKIEn7AIvDQ==', '2019-11-10', '0001-01-01'),
(179, 'SIkMxoMaFEWScSWULdg8+g==', '2019-11-11', '0001-01-01'),
(179, 'ArW58ImU3UiqCcHwBV9qkQ==', '2019-11-11', '0001-01-01'),
(179, 'EnL5ohRmy0+UxcbMSRIV9w==', '2019-11-11', '0001-01-01'),
(179, 'UOOI/GTfx0a9zwD9/QeYCg==', '2019-11-11', '0001-01-01'),
(179, 'IjiS2ZrfN0qpzDIwBu1mDg==', '2019-11-11', '0001-01-01'),
(179, '3ZZivS6k2EKY/ukCE5xLNQ==', '2019-11-11', '0001-01-01'),
(179, 'JHru6zys2UOn4BGGQ00Iow==', '2019-11-11', '0001-01-01'),
(179, 'o7G8l1nDQESqqspKTbr6dQ==', '2019-11-11', '0001-01-01'),
(179, 'XDlw+ohGj0uQr0lHDUBIzQ==', '2019-11-11', '0001-01-01'),
(179, 'L5VZmSn/fUeo0tprlbGZDg==', '2019-11-11', '0001-01-01'),
(179, '9KlVGG7jYEqJP4/ddd2fXQ==', '2019-11-11', '0001-01-01'),
(179, 'zjF41ZiCRk+WudHG83iCLQ==', '2019-11-11', '0001-01-01'),
(179, 'J3LcEldYNUSK0temmobAxw==', '2019-11-11', '0001-01-01'),
(179, 'C7iG0wuyv0i+18TDdY9yRQ==', '2019-11-11', '0001-01-01'),
(179, '1aCfd5V8YkWgJVLZ9V05Fw==', '2019-11-11', '0001-01-01'),
(179, 'HKE3HSs8vEKbh8qOJdU4cw==', '2019-11-11', '0001-01-01'),
(179, 'fWIz3qnjxUOnJ5v+rt1+0A==', '2019-11-12', '0001-01-01'),
(173, '60Y2WE/D7UeGl4Hcoly1vA==', '2019-11-12', '0001-01-01'),
(173, '1NRJ5jC+pES8UEzWlZCeKQ==', '2019-11-12', '0001-01-01'),
(173, 'E9DSb8e5v0Od9ZvTzkT3Kg==', '2019-11-12', '0001-01-01'),
(173, 'usgsVkCOvE+zScU5Bm1kMQ==', '2019-11-12', '0001-01-01'),
(173, '+tcwwDVNbkaTbwY31tIoPA==', '2019-11-12', '0001-01-01'),
(173, 'aOjP43GH4UuVqIw5Dn++5A==', '2019-11-12', '0001-01-01'),
(173, '0WPHEmkkMUCMuD7KfFRRUQ==', '2019-11-12', '0001-01-01'),
(173, 'cMGXGkng70eAKbQ82Z2Hug==', '2019-11-12', '0001-01-01'),
(173, '5t425RYpPkOFe5ceUdznhw==', '2019-11-12', '0001-01-01'),
(173, 'C5rU0h8SmkiNbTww9xOGyQ==', '2019-11-12', '0001-01-01'),
(173, '5R/sX9M96k2Hoc7hH9N69Q==', '2019-11-12', '0001-01-01'),
(173, '0Iuw82rmmEOZAxKjjHd4ag==', '2019-11-12', '0001-01-01'),
(173, '3HYdcVG5e0eug+Y1DdRBHQ==', '2019-11-12', '0001-01-01'),
(173, 'E4ZIbvE41UCiZxmyYH9eRA==', '2019-11-12', '0001-01-01'),
(173, 'TT/K+7+cCkavsP0B08eVWA==', '2019-11-12', '0001-01-01');

-- --------------------------------------------------------

--
-- Structure de la table `resource`
--

CREATE TABLE `resource` (
  `id` int(11) NOT NULL,
  `name` varchar(48) COLLATE utf8_unicode_ci NOT NULL,
  `resource_type_id` int(11) NOT NULL DEFAULT '0',
  `available_in_player_map` int(11) NOT NULL DEFAULT '0' COMMENT 'is the ressource can be found in the player map'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `resource`
--

INSERT INTO `resource` (`id`, `name`, `resource_type_id`, `available_in_player_map`) VALUES
(1, 'Broccoli', 1, 1),
(2, 'Minerals', 1, 1),
(3, 'Metals', 1, 1),
(4, 'Animal food', 1, 0),
(5, 'Preserves', 1, 0),
(6, 'Building materials', 1, 0),
(7, 'Energy', 3, 0),
(8, 'Population', 3, 0);

-- --------------------------------------------------------

--
-- Structure de la table `resource_bag`
--

CREATE TABLE `resource_bag` (
  `id` int(11) NOT NULL,
  `resource_id` int(11) NOT NULL,
  `amount` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT;

--
-- Déchargement des données de la table `resource_bag`
--

INSERT INTO `resource_bag` (`id`, `resource_id`, `amount`) VALUES
(1, 6, 200),
(2, 6, 600),
(3, 6, 1800),
(4, 6, 250),
(5, 6, 750),
(6, 6, 2100),
(7, 6, 300),
(8, 6, 900),
(9, 6, 2700),
(10, 6, 200),
(11, 6, 600),
(12, 6, 1800),
(13, 6, 300),
(14, 6, 900),
(15, 6, 2700),
(16, 6, 300),
(17, 6, 900),
(18, 6, 2700),
(19, 6, 1500),
(20, 6, 3000),
(21, 6, 2500),
(22, 7, 10),
(23, 7, 20),
(24, 3, 10),
(25, 3, 15),
(26, 8, 5),
(27, 7, 1),
(28, 7, 10),
(28, 8, 10),
(29, 7, 1),
(30, 7, 1),
(31, 7, 2),
(32, 7, 3),
(33, 7, 3),
(34, 7, 4),
(35, 7, 3),
(36, 7, 3),
(37, 7, 4),
(38, 7, 2),
(39, 7, 2),
(40, 7, 3),
(41, 7, 2),
(42, 7, 2),
(43, 7, 3),
(44, 7, 2),
(45, 7, 2),
(46, 7, 3),
(47, 1, 20),
(48, 1, 30),
(49, 1, 40),
(50, 3, 10),
(50, 4, 10),
(51, 3, 15),
(51, 4, 15),
(52, 3, 20),
(52, 4, 20),
(53, 2, 10),
(53, 3, 10),
(53, 4, 10),
(54, 2, 15),
(54, 3, 15),
(54, 4, 15),
(55, 2, 20),
(55, 3, 20),
(55, 4, 20);

-- --------------------------------------------------------

--
-- Structure de la table `resource_type`
--

CREATE TABLE `resource_type` (
  `id` int(11) NOT NULL,
  `name` varchar(30) COLLATE utf8_unicode_ci NOT NULL,
  `default_maximum` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `resource_type`
--

INSERT INTO `resource_type` (`id`, `name`, `default_maximum`) VALUES
(0, 'Undefined', 0),
(1, 'Common', 3000),
(2, 'Rare', 1500),
(3, 'Persistent', 99999);

-- --------------------------------------------------------

--
-- Structure de la table `server_instance`
--

CREATE TABLE `server_instance` (
  `name` tinytext COLLATE utf8_unicode_ci NOT NULL,
  `environment` tinytext COLLATE utf8_unicode_ci NOT NULL,
  `host` tinytext COLLATE utf8_unicode_ci NOT NULL,
  `port` smallint(6) NOT NULL,
  `token` tinytext COLLATE utf8_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Déchargement des données de la table `server_instance`
--

INSERT INTO `server_instance` (`name`, `environment`, `host`, `port`, `token`) VALUES
('LoginServer', 'DEV_DAVID', '127.0.0.1', 4200, '47KJshiWGEeV7gwJoapoqw=='),
('GameServer1', 'DEV_DAVID', '127.0.0.1', 4210, 'gioRAobyN06SuRfpbbPpyw==');

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `building`
--
ALTER TABLE `building`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `building_center`
--
ALTER TABLE `building_center`
  ADD PRIMARY KEY (`building_id`);

--
-- Index pour la table `building_center_level`
--
ALTER TABLE `building_center_level`
  ADD PRIMARY KEY (`level_id`,`building_id`);

--
-- Index pour la table `building_level_cost`
--
ALTER TABLE `building_level_cost`
  ADD PRIMARY KEY (`building_id`,`level_id`);

--
-- Index pour la table `building_passive`
--
ALTER TABLE `building_passive`
  ADD PRIMARY KEY (`building_id`);

--
-- Index pour la table `building_passive_level`
--
ALTER TABLE `building_passive_level`
  ADD PRIMARY KEY (`building_id`,`level_id`);

--
-- Index pour la table `building_producer`
--
ALTER TABLE `building_producer`
  ADD PRIMARY KEY (`building_id`);

--
-- Index pour la table `building_producer_level`
--
ALTER TABLE `building_producer_level`
  ADD PRIMARY KEY (`building_id`,`resource_id_produced`,`level_id`);

--
-- Index pour la table `building_state`
--
ALTER TABLE `building_state`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `city_level`
--
ALTER TABLE `city_level`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `map_element`
--
ALTER TABLE `map_element`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `map_element_type`
--
ALTER TABLE `map_element_type`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `map_extent`
--
ALTER TABLE `map_extent`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `map_extent_element`
--
ALTER TABLE `map_extent_element`
  ADD PRIMARY KEY (`map_extent_id`,`position`);

--
-- Index pour la table `market_level`
--
ALTER TABLE `market_level`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `market_resource_ratio`
--
ALTER TABLE `market_resource_ratio`
  ADD PRIMARY KEY (`resource_id_given`,`resource_id_received`) USING BTREE;

--
-- Index pour la table `player`
--
ALTER TABLE `player`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `player_building`
--
ALTER TABLE `player_building`
  ADD PRIMARY KEY (`player_id`,`building_id`,`building_number`);

--
-- Index pour la table `player_building_center`
--
ALTER TABLE `player_building_center`
  ADD PRIMARY KEY (`player_id`,`building_id`,`building_number`);

--
-- Index pour la table `player_building_passive`
--
ALTER TABLE `player_building_passive`
  ADD PRIMARY KEY (`player_id`,`building_id`,`building_number`);

--
-- Index pour la table `player_building_producer`
--
ALTER TABLE `player_building_producer`
  ADD PRIMARY KEY (`player_id`,`building_id`,`building_number`);

--
-- Index pour la table `player_city`
--
ALTER TABLE `player_city`
  ADD PRIMARY KEY (`player_id`);

--
-- Index pour la table `player_map`
--
ALTER TABLE `player_map`
  ADD PRIMARY KEY (`player_id`);

--
-- Index pour la table `player_map_extent`
--
ALTER TABLE `player_map_extent`
  ADD PRIMARY KEY (`player_id`,`map_extent_id`);

--
-- Index pour la table `player_market`
--
ALTER TABLE `player_market`
  ADD UNIQUE KEY `player_id` (`player_id`);

--
-- Index pour la table `player_market_trades`
--
ALTER TABLE `player_market_trades`
  ADD PRIMARY KEY (`player_id`,`creation`) USING BTREE;

--
-- Index pour la table `player_resource_bag`
--
ALTER TABLE `player_resource_bag`
  ADD PRIMARY KEY (`player_id`,`resource_id`);

--
-- Index pour la table `player_resource_bag_default`
--
ALTER TABLE `player_resource_bag_default`
  ADD PRIMARY KEY (`resource_id`);

--
-- Index pour la table `resource`
--
ALTER TABLE `resource`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `resource_bag`
--
ALTER TABLE `resource_bag`
  ADD PRIMARY KEY (`id`,`resource_id`);

--
-- Index pour la table `resource_type`
--
ALTER TABLE `resource_type`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT pour les tables déchargées
--

--
-- AUTO_INCREMENT pour la table `building`
--
ALTER TABLE `building`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT pour la table `building_state`
--
ALTER TABLE `building_state`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT pour la table `player`
--
ALTER TABLE `player`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=180;

--
-- AUTO_INCREMENT pour la table `resource`
--
ALTER TABLE `resource`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
