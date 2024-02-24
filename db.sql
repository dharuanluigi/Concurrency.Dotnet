CREATE TABLE clients(
    id INT PRIMARY KEY AUTO_INCREMENT,
    c_limit INT NOT NULL,
    balance INT NOT NULL
);

CREATE TABLE transactions(
    id INT PRIMARY KEY AUTO_INCREMENT,
    t_value INT NOT NULL,
    t_type ENUM('c', 'd') NOT NULL,
    t_desc VARCHAR(10) NOT NULL,
    process_at DATETIME NOT NULL,
    client_id INT,

    FOREIGN KEY (client_id)
    REFERENCES clients(id)
);

-- insert clients
INSERT INTO clients VALUES(NULL, 100000, 0);
INSERT INTO clients VALUES(NULL, 80000, 0);
INSERT INTO clients VALUES(NULL, 1000000, 0);
INSERT INTO clients VALUES(NULL, 10000000, 0);
INSERT INTO clients VALUES(NULL, 500000, 0);
