CREATE TABLE useremail (
    id INT AUTO_INCREMENT PRIMARY KEY,
    email VARCHAR(255) UNIQUE NOT NULL
);
CREATE TABLE preferences (
    id INT AUTO_INCREMENT PRIMARY KEY,
    preference VARCHAR(255) UNIQUE NOT NULL
);
CREATE TABLE userpreferences (
    user_id INT,
    preference_id INT,
    FOREIGN KEY (user_id) REFERENCES useremail(id),
    FOREIGN KEY (preference_id) REFERENCES preferences(id),
    PRIMARY KEY (user_id, preference_id)
);