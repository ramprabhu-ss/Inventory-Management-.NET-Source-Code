-- Create todo_tasks table
CREATE TABLE IF NOT EXISTS todo_tasks (
	task_id INT AUTO_INCREMENT PRIMARY KEY,
	title VARCHAR(255) NOT NULL,
	description TEXT,
	end_date DATE,
	completed BOOLEAN DEFAULT FALSE,
	created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
	updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- If the table already exists, add the end_date column
ALTER TABLE todo_tasks ADD COLUMN IF NOT EXISTS end_date DATE AFTER description;

-- Insert sample data (optional)
INSERT INTO todo_tasks (title, description, end_date, completed) VALUES
('Setup Database', 'Configure MySQL database for inventory management', '2024-02-15', TRUE),
('Create Product Categories', 'Add initial product categories to the system', '2024-02-20', FALSE),
('Test GST Calculation', 'Verify GST calculations are working correctly', '2024-02-25', FALSE);
