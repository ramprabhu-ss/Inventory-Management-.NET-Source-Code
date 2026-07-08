-- Migration script to add end_date column to existing todo_tasks table
-- Run this if you already created the todo_tasks table without the end_date column

USE inventory_management;

-- Add the end_date column if it doesn't exist
ALTER TABLE todo_tasks 
ADD COLUMN end_date DATE AFTER description;

-- You can optionally update existing records with sample end dates
-- UPDATE todo_tasks SET end_date = DATE_ADD(created_at, INTERVAL 7 DAY) WHERE end_date IS NULL;
