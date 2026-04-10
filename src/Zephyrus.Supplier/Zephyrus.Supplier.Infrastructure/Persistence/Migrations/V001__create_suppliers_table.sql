CREATE TABLE IF NOT EXISTS suppliers
(
    id             UUID         PRIMARY KEY,
    name           VARCHAR(200) NOT NULL UNIQUE,
    contact_person VARCHAR(200) NOT NULL,
    email          VARCHAR(200) NOT NULL,
    phone          VARCHAR(50)  NOT NULL,
    is_active      BOOLEAN      NOT NULL DEFAULT TRUE,
    date_created   TIMESTAMPTZ  NOT NULL,
    date_updated   TIMESTAMPTZ  NOT NULL
);
