/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50612
Source Host           : localhost:3306
Source Database       : cubano_local

Target Server Type    : MYSQL
Target Server Version : 50612
File Encoding         : 65001

Date: 2014-01-05 15:18:59
*/

SET FOREIGN_KEY_CHECKS=0;
CREATE DATABASE cubano_local;
USE  cubano_local;

-- ----------------------------
-- Table structure for aperturas
-- ----------------------------
DROP TABLE IF EXISTS `aperturas`;
CREATE TABLE `aperturas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `fecha` datetime DEFAULT NULL,
  `tienda` int(11) DEFAULT NULL,
  `monto` float DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of aperturas
-- ----------------------------

-- ----------------------------
-- Table structure for clientes
-- ----------------------------
DROP TABLE IF EXISTS `clientes`;
CREATE TABLE `clientes` (
  `idCliente` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `nombre` varchar(200) NOT NULL,
  `rfc` varchar(100) NOT NULL,
  `calle` varchar(100) NOT NULL,
  `numero` varchar(100) NOT NULL,
  `colonia` varchar(100) NOT NULL,
  `ciudad` varchar(100) NOT NULL,
  `estado` varchar(100) NOT NULL,
  `cp` varchar(100) NOT NULL,
  `telefono` varchar(100) NOT NULL,
  `idtienda` int(11) DEFAULT NULL,
  `email` varchar(100) NOT NULL,
  PRIMARY KEY (`idCliente`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of clientes
-- ----------------------------

-- ----------------------------
-- Table structure for estatus
-- ----------------------------
DROP TABLE IF EXISTS `estatus`;
CREATE TABLE `estatus` (
  `idEstatus` smallint(5) unsigned NOT NULL AUTO_INCREMENT,
  `estatus` varchar(100) NOT NULL,
  PRIMARY KEY (`idEstatus`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of estatus
-- ----------------------------
INSERT INTO `estatus` VALUES ('1', 'Generado');
INSERT INTO `estatus` VALUES ('2', 'Cancelado');
INSERT INTO `estatus` VALUES ('3', 'Facturado');

-- ----------------------------
-- Table structure for extra_iva
-- ----------------------------
DROP TABLE IF EXISTS `extra_iva`;
CREATE TABLE `extra_iva` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `monto` float DEFAULT NULL,
  `tienda_id` int(11) DEFAULT NULL,
  `fecha` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of extra_iva
-- ----------------------------

-- ----------------------------
-- Table structure for facturas
-- ----------------------------
DROP TABLE IF EXISTS `facturas`;
CREATE TABLE `facturas` (
  `idFactura` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `idFacturaLocal` int(10) unsigned NOT NULL,
  `idTienda` smallint(5) unsigned NOT NULL,
  `idCliente` smallint(5) unsigned NOT NULL,
  `idEstatus` smallint(5) unsigned NOT NULL,
  `fecha` datetime NOT NULL,
  `folio` smallint(5) unsigned NOT NULL,
  `monto` double NOT NULL,
  `metodo` int(11) DEFAULT NULL,
  `cuenta` char(255) DEFAULT NULL,
  `vendedor` int(11) DEFAULT NULL,
  `folio_fiscal` varchar(255) DEFAULT NULL,
  `xml_timbrado` text,
  PRIMARY KEY (`idFactura`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of facturas
-- ----------------------------

-- ----------------------------
-- Table structure for facturas_detalle
-- ----------------------------
DROP TABLE IF EXISTS `facturas_detalle`;
CREATE TABLE `facturas_detalle` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `folio_factura` int(11) NOT NULL,
  `descripcion` varchar(10) NOT NULL,
  `cantidad` smallint(5) unsigned NOT NULL,
  `precio` double NOT NULL,
  `fecha` datetime DEFAULT NULL,
  `idTienda` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of facturas_detalle
-- ----------------------------

-- ----------------------------
-- Table structure for foliossat
-- ----------------------------
DROP TABLE IF EXISTS `foliossat`;
CREATE TABLE `foliossat` (
  `idFolioSat` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `idTienda` smallint(5) unsigned NOT NULL,
  `folioSat` int(10) unsigned NOT NULL,
  `folio` int(10) unsigned NOT NULL,
  `folio_factura` int(10) DEFAULT NULL,
  PRIMARY KEY (`idFolioSat`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of foliossat
-- ----------------------------
INSERT INTO `foliossat` VALUES ('1', '1', '1', '1', '0');
INSERT INTO `foliossat` VALUES ('2', '2', '1', '1', '0');
INSERT INTO `foliossat` VALUES ('3', '3', '1', '1', '0');
INSERT INTO `foliossat` VALUES ('4', '4', '1', '1', '0');
INSERT INTO `foliossat` VALUES ('5', '5', '1', '1', '0');
-- ----------------------------
-- Table structure for gastos
-- ----------------------------
DROP TABLE IF EXISTS `gastos`;
CREATE TABLE `gastos` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `idTienda` int(11) DEFAULT NULL,
  `descripcion` char(250) DEFAULT NULL,
  `monto` float DEFAULT NULL,
  `fecha` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of gastos
-- ----------------------------

-- ----------------------------
-- Table structure for metodos_de_pago
-- ----------------------------
DROP TABLE IF EXISTS `metodos_de_pago`;
CREATE TABLE `metodos_de_pago` (
  `id` int(11) NOT NULL,
  `description` char(255) DEFAULT NULL,
  `efectivo` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of metodos_de_pago
-- ----------------------------
INSERT INTO `metodos_de_pago` VALUES ('1', 'EFECTIVO', '1');
INSERT INTO `metodos_de_pago` VALUES ('2', 'DEPOSITO', '0');
INSERT INTO `metodos_de_pago` VALUES ('3', 'CHEQUE', '0');
INSERT INTO `metodos_de_pago` VALUES ('4', 'TRANSFERENCIA', '0');

-- ----------------------------
-- Table structure for producto_tienda
-- ----------------------------
DROP TABLE IF EXISTS `producto_tienda`;
CREATE TABLE `producto_tienda` (
  `idproductoTienda` varchar(13) COLLATE utf8_unicode_ci NOT NULL,
  `nombre` varchar(45) COLLATE utf8_unicode_ci NOT NULL,
  `fecha_alta` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `activo` tinyint(1) NOT NULL,
  `precio` decimal(5,2) NOT NULL,
  `costo` decimal(5,2) NOT NULL,
  `medida` int(11) NOT NULL,
  `categorias_id` varchar(7) COLLATE utf8_unicode_ci NOT NULL,
  `tienda1` int(11) NOT NULL,
  `tienda2` int(11) NOT NULL,
  `tienda3` int(11) NOT NULL,
  `tienda4` int(11) NOT NULL,
  `tienda5` int(11) NOT NULL,
  PRIMARY KEY (`idproductoTienda`),
  KEY `fk_productoTienda_categorias1` (`categorias_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Records of producto_tienda
-- ----------------------------

-- ----------------------------
-- Table structure for tiendas
-- ----------------------------
DROP TABLE IF EXISTS `tiendas`;
CREATE TABLE `tiendas` (
  `idTienda` smallint(5) unsigned NOT NULL AUTO_INCREMENT,
  `tienda` varchar(200) NOT NULL,
  `piezas_mayoreo` int(11) DEFAULT '30',
  `nombre` char(255) DEFAULT NULL,
  `rfc` char(20) DEFAULT NULL,
  `estado` char(255) DEFAULT NULL,
  `ciudad` char(255) DEFAULT NULL,
  `colonia` char(255) DEFAULT NULL,
  `curp` char(50) DEFAULT NULL,
  `cp` char(50) DEFAULT NULL,
  `calle` char(255) DEFAULT NULL,
  `telefono` char(255) DEFAULT NULL,
  `factura_generica_generada` tinyint(4) DEFAULT '0',
  `padre` int(11) DEFAULT NULL,
  `prefijo` char(50) DEFAULT NULL,
  `saldo_apertura` float DEFAULT NULL,
  `logo` char(255) DEFAULT NULL,
  `cbb` char(255) DEFAULT NULL,
  `cer_file` text,
  `key_file` text,
  `pass` char(255) DEFAULT NULL,
  `lic` varchar(255) DEFAULT NULL,
  `cuenta_nip` char(255) DEFAULT NULL,
  `pass_pac` char(255) DEFAULT NULL,
  `testing` bit(1) DEFAULT b'1',
  PRIMARY KEY (`idTienda`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of tiendas
-- ----------------------------

-- ----------------------------
-- Table structure for vendedores
-- ----------------------------
DROP TABLE IF EXISTS `vendedores`;
CREATE TABLE `vendedores` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` char(255) DEFAULT NULL,
  `tienda` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of vendedores
-- ----------------------------

-- ----------------------------
-- Table structure for ventas
-- ----------------------------
DROP TABLE IF EXISTS `ventas`;
CREATE TABLE `ventas` (
  `idVenta` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `idVentaLocal` int(11) unsigned NOT NULL,
  `idEstatus` smallint(5) unsigned NOT NULL,
  `idTienda` smallint(5) unsigned NOT NULL,
  `fecha` datetime NOT NULL,
  `monto` double NOT NULL,
  `foliosat` smallint(5) unsigned NOT NULL,
  `verificada` tinyint(1) NOT NULL,
  `metodo` int(11) DEFAULT NULL,
  `vendedor` int(11) DEFAULT NULL,
  `facturada` smallint(5) NOT NULL COMMENT '0: No facturada, 1: Facturada a un cliente, 2: Facturada en Genérica, 3: Factura en cliente y en la factura genérica',
  `folio` smallint(5) unsigned NOT NULL,
  `folio_factura` int(11) DEFAULT NULL,
  PRIMARY KEY (`idVenta`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of ventas
-- ----------------------------

-- ----------------------------
-- Table structure for ventas_detalle
-- ----------------------------
DROP TABLE IF EXISTS `ventas_detalle`;
CREATE TABLE `ventas_detalle` (
  `idDetalleVenta` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `idVenta` int(11) NOT NULL,
  `idproductoTienda` varchar(10) NOT NULL,
  `cantidad` smallint(5) unsigned NOT NULL,
  `precio` double NOT NULL,
  `fecha` datetime DEFAULT NULL,
  PRIMARY KEY (`idDetalleVenta`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of ventas_detalle
-- ----------------------------
