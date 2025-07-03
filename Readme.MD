# Repositorio Front: https://github.com/Alexis5631/FrontProyectFinal.git

# 🚗 Sistema Automotriz - AutoTaller 🛠️

¡Bienvenido al proyecto **AutoTaller**! 🎉

Este sistema está diseñado para la **gestión integral de un taller automotriz**. Permite administrar todos los aspectos clave de un taller, desde la recepción de vehículos hasta la facturación y auditoría de procesos. Todo en un solo lugar, fácil de usar y pensado para optimizar tu trabajo diario. 🧾🔧

---

## 🏢 ¿A quién está dirigido?

A talleres automotrices que buscan digitalizar y mejorar la gestión de sus operaciones, clientes y recursos.

---

## 🧩 Módulos principales

### 👥 Gestión de Clientes
- Registro, edición y consulta de clientes.
- Visualización del historial de servicios de cada cliente.

### 🚗 Gestión de Vehículos
- Asociación de vehículos a clientes.
- Registro de datos técnicos y características de cada vehículo.
- Consulta de historial de reparaciones y servicios.

### 📝 Órdenes de Servicio
- Creación de órdenes para trabajos de mantenimiento o reparación.
- Asignación de técnicos y seguimiento del estado de la orden (pendiente, en proceso, finalizada).
- Detalle de diagnósticos y servicios realizados.

### 🏷️ Inventario y Repuestos
- Control de stock de repuestos y materiales.
- Registro de entradas y salidas de inventario.
- Asociación de repuestos utilizados a cada orden de servicio.

### 💵 Facturación y Pagos
- Generación automática de facturas por servicios realizados.
- Registro de pagos y control de cuentas por cobrar.

### 🛡️ Roles y Permisos
- Gestión de usuarios del sistema (administradores, técnicos, cajeros, etc.).
- Asignación de roles y permisos personalizados para cada usuario.

### 🔍 Auditoría y Seguridad
- Registro de todas las acciones importantes realizadas en el sistema.
- Seguimiento de cambios y operaciones para mayor transparencia y control.

---

## 🗂️ Entidades principales
- **Cliente**: Persona que solicita servicios al taller.
- **Vehículo**: Automóvil registrado en el sistema, asociado a un cliente.
- **Orden de Servicio**: Solicitud de trabajo para un vehículo.
- **Detalle de Diagnóstico**: Información sobre problemas detectados y soluciones propuestas.
- **Inventario/Repuesto**: Materiales y repuestos disponibles en el taller.
- **Factura**: Documento generado por los servicios prestados.
- **Usuario/Rol**: Personas que usan el sistema y sus permisos.

---

## 🔄 Flujo general del sistema
1. **Registro del cliente y su vehículo** 👤➡️🚗
2. **Creación de una orden de servicio** 📝
3. **Diagnóstico y asignación de repuestos** 🔧
4. **Actualización del inventario** 📦
5. **Finalización del servicio y generación de factura** ✅💵
6. **Auditoría y seguimiento de todas las operaciones** 🔍

---

## 🖥️ Tecnologías utilizadas
- **ASP.NET Core** (API REST)
- **Entity Framework Core** (ORM)
- **SQL Server** (Base de datos)
- **Arquitectura en capas** (Domain, Application, Infrastructure, API)

---

## 🎯 Objetivo

Facilitar la administración de talleres automotrices, optimizando procesos, reduciendo errores y mejorando la experiencia tanto para los empleados como para los clientes.

---

¡Esperamos que disfrutes usando **AutoTaller** tanto como nosotros disfrutamos creándolo! 🚀✨

---

## 📁 Estructura del Proyecto

```plaintext
ProyectoFinal/
│
├── ApiProject/           # API principal (ASP.NET Core)
│   ├── Controllers/      # Controladores de la API 🚦
│   ├── Extensions/       # Extensiones y helpers 🧩
│   ├── Helpers/          # Utilidades y middlewares 🛠️
│   ├── Profiles/         # Perfiles de mapeo (AutoMapper) 🔄
│   ├── Services/         # Servicios de negocio ⚙️
│   ├── Program.cs        # Configuración de inicio 🏁
│   └── ...
│
├── Application/          # Lógica de aplicación y DTOs 📦
│   ├── DTOs/             # Objetos de transferencia de datos 📄
│   ├── Interfaces/       # Interfaces de servicios y repositorios 🔌
│   ├── Services/         # Servicios de aplicación 🧠
│   └── ...
│
├── Domain/               # Entidades del dominio y lógica central 🏷️
│   ├── Entities/         # Clases de entidades principales 🚗
│   └── ...
│
├── Infrastructure/       # Acceso a datos y configuración de base de datos 🗄️
│   ├── Configuration/    # Configuración de entidades (EF Core) ⚙️
│   ├── Data/             # Contexto de base de datos y migraciones 🗃️
│   ├── Repositories/     # Implementaciones de repositorios 📚
│   ├── UnitOfWork/       # Patrón Unit of Work 🔄
│   └── ...
│
├── db/                   # Scripts de base de datos y migraciones 📝
│
├── AutoTaller.sln        # Solución principal de Visual Studio 🧩
└── Readme.MD             # Documentación del proyecto 📖
```

---

# Autores ✒️

- Joinner Alexis Silva Sanchez  
- Juan Pablo Pinilla Guzman  
