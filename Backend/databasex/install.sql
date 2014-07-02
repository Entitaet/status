CREATE TABLE IF NOT EXISTS `status` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `key` TINYTEXT CHARACTER SET 'utf8' COLLATE 'utf8_unicode_ci' NULL,
  `value` TINYTEXT CHARACTER SET 'utf8' COLLATE 'utf8_unicode_ci' NULL,
  PRIMARY KEY (`id`));
  
CREATE TABLE IF NOT EXISTS `token` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `token` TINYTEXT NULL,
  `description` TINYTEXT NULL,
  `read` BIT NULL,
  `write` BIT NULL,
  PRIMARY KEY (`id`));