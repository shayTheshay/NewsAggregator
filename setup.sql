CREATE TABLE useremail (
    id INT AUTO_INCREMENT PRIMARY KEY,
    email VARCHAR(255) UNIQUE NOT NULL
);
CREATE TABLE preferences (
    id INT AUTO_INCREMENT PRIMARY KEY,
    preference VARCHAR(255) NOT NULL
);
