CREATE TABLE IF NOT EXISTS categories
(
    id           UUID         PRIMARY KEY,
    name         VARCHAR(100) NOT NULL UNIQUE,
    parent_id    UUID         REFERENCES categories(id) ON DELETE SET NULL,
    date_created TIMESTAMPTZ  NOT NULL,
    date_updated TIMESTAMPTZ  NOT NULL
);
