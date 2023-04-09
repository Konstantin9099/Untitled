CREATE DATABASE Untitled;

USE Untitled ;

CREATE TABLE `auth` (
  `auth_id` int PRIMARY KEY AUTO_INCREMENT,
  `auth_log` varchar(20),
  `auth_pwd` text,
  `auth_role` int
);

CREATE TABLE `rols` (
  `role_id` int PRIMARY KEY AUTO_INCREMENT,
  `role_name` varchar(20)
);

CREATE TABLE `users` (
  `user_id` int PRIMARY KEY AUTO_INCREMENT,
  `user_name` varchar(60),
  `user_birth` date,
  `user_auth` int
);

CREATE TABLE `items` (
  `item_id` int PRIMARY KEY AUTO_INCREMENT,
  `item_name` varchar(20),
  `item_cost` double
);

CREATE TABLE `cort` (
  `order_id` int,
  `item_id` int,
  `amount` int
);

CREATE TABLE `orders` (
  `order_id` int PRIMARY KEY AUTO_INCREMENT,
  `order_date` date,
  `order_user` int
);

ALTER TABLE `users` ADD FOREIGN KEY (`user_auth`) REFERENCES `auth` (`auth_id`);

ALTER TABLE `auth` ADD FOREIGN KEY (`auth_role`) REFERENCES `rols` (`role_id`);

ALTER TABLE `orders` ADD FOREIGN KEY (`order_user`) REFERENCES `users` (`user_id`);

ALTER TABLE `cort` ADD FOREIGN KEY (`order_id`) REFERENCES `orders` (`order_id`);

ALTER TABLE `cort` ADD FOREIGN KEY (`item_id`) REFERENCES `items` (`item_id`);
