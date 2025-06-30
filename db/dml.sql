-- Roles
INSERT INTO public.role (description, created_at, updated_at)
VALUES 
('Administrator', CURRENT_DATE, CURRENT_DATE),
('Mechanic', CURRENT_DATE, CURRENT_DATE),
('Receptionist', CURRENT_DATE, CURRENT_DATE);

-- Users
INSERT INTO public.users (name, lastname, email, passwordhash, username, created_at, updated_at)
VALUES 
('John', 'Doe', 'john.doe@example.com', 'hashed_pw_1', 'jdoe', CURRENT_DATE, CURRENT_DATE),
('Jane', 'Smith', 'jane.smith@example.com', 'hashed_pw_2', 'jsmith', CURRENT_DATE, CURRENT_DATE);

-- User Roles
INSERT INTO public.user_roles ("IdUser", "IdRole")
VALUES 
(1, 1),
(2, 2);

-- Clients
INSERT INTO public.clients (name, lastname, phone, email, identification, created_at, updated_at)
VALUES 
('Michael', 'Johnson', '1234567890', 'michael.j@example.com', 'CC123', CURRENT_DATE, CURRENT_DATE),
('Emily', 'Clark', '0987654321', 'emily.c@example.com', 'CC456', CURRENT_DATE, CURRENT_DATE);

-- Vehicles
INSERT INTO public.vehicles (id_client, brand, model, year, serial_vin, mileage, "CreatedAt", "UpdatedAt")
VALUES 
(1, 'Toyota', 'Corolla', 2020, '1HGCM82633A123456', 30000, CURRENT_DATE, CURRENT_DATE),
(2, 'Honda', 'Civic', 2022, '2HGCM82633A654321', 15000, CURRENT_DATE, CURRENT_DATE);

-- States
INSERT INTO public.states (state_type, created_at, updated_at)
VALUES 
('Pending', CURRENT_DATE, CURRENT_DATE),
('In Progress', CURRENT_DATE, CURRENT_DATE),
('Completed', CURRENT_DATE, CURRENT_DATE);

-- Service Types
INSERT INTO public.service_types (duration, description, created_at, updated_at)
VALUES 
(60, 'Oil Change', CURRENT_DATE, CURRENT_DATE),
(120, 'Brake Replacement', CURRENT_DATE, CURRENT_DATE);

-- Service Orders
INSERT INTO public.service_orders (id_vehicle, id_user, id_service_type, id_state, entry_date, exit_date, client_message, created_at, updated_at)
VALUES 
(1, 2, 1, 1, '2025-06-25', '2025-06-26', 'Please check the brakes.', CURRENT_DATE, CURRENT_DATE),
(2, 2, 2, 2, '2025-06-27', '2025-06-28', 'Oil change required.', CURRENT_DATE, CURRENT_DATE);

-- Replacements
INSERT INTO public.replacements (code, description, stock_quantity, minimum_stock, unit_price, category, created_at, updated_at)
VALUES 
('R001', 'Brake Pads', 50, 10, 25.0, 'Brakes', CURRENT_DATE, CURRENT_DATE),
('R002', 'Engine Oil', 100, 20, 15.0, 'Oil', CURRENT_DATE, CURRENT_DATE);

-- Order Details
INSERT INTO public.order_details (id_order, id_replacement, quantity, total_cost, created_at, updated_at)
VALUES
(1, 1, 4, 100.0, CURRENT_DATE, CURRENT_DATE),
(2, 2, 2, 30.0, CURRENT_DATE, CURRENT_DATE);

-- Invoices
INSERT INTO public.invoices (service_order_id, issue_date, labor_total, replacements_total, total_amount, created_at, updated_at)
VALUES
(1, '2025-06-26', 80.0, 100.0, 180.0, CURRENT_DATE, CURRENT_DATE),
(2, '2025-06-28', 50.0, 30.0, 80.0, CURRENT_DATE, CURRENT_DATE);

-- Diagnostics
INSERT INTO public.diagnostics (id_user, description, created_at, updated_at)
VALUES 
(2, 'Front brake pads worn out', '2025-06-25', '2025-06-25'),
(2, 'Engine oil low level', '2025-06-27', '2025-06-27');

-- Details Diagnostics
INSERT INTO public.details_diagnostics (id_service_order, id_diagnostic)
VALUES 
(1, 1),
(2, 2);

-- Inventories
INSERT INTO public.inventories (name, created_at, updated_at)
VALUES 
('Brake Stock', '2025-06-20', '2025-06-20'),
('Oil Stock', '2025-06-20', '2025-06-20');

-- Inventory Details
INSERT INTO public.inventory_details (id_order, id_inventory, quantity, created_at, updated_at)
VALUES
(1, 1, 10, '2025-06-25', '2025-06-25'),
(2, 2, 5, '2025-06-27', '2025-06-27');

-- Specializations
INSERT INTO public.specializations (name, created_at, updated_at)
VALUES 
('Brake Systems', '2025-06-01', '2025-06-01'),
('Engine Maintenance', '2025-06-01', '2025-06-01');

-- User Specializations
INSERT INTO public.user_specializations ("IdUser", "IdSpecialization")
VALUES 
(2, 1),
(2, 2);

-- Auditories
INSERT INTO public.auditories (entity_name, change_type, "user", date, "UserId", created_at, updated_at)
VALUES 
('service_orders', 'INSERT', 'admin', '2025-06-25 10:30:00', 1, '2025-06-25', '2025-06-25'),
('invoices', 'INSERT', 'admin', '2025-06-28 09:15:00', 1, '2025-06-28', '2025-06-28');

-- Refresh Tokens
INSERT INTO public.refresh_tokens (user_id, token, expires, created, revoked, "createdAt", "updatedAt")
VALUES 
(1, 'token123', '2025-07-10 12:00:00', '2025-06-25 12:00:00', NULL, '2025-06-25', '2025-06-25');
