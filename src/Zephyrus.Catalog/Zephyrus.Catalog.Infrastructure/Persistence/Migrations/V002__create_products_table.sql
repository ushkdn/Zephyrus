CREATE TABLE IF NOT EXISTS products
(
    id           UUID          PRIMARY KEY,
    name         VARCHAR(200)  NOT NULL UNIQUE,
    description  TEXT          NOT NULL DEFAULT '',
    unit         VARCHAR(20)   NOT NULL,
    category_id  UUID          NOT NULL REFERENCES categories(id) ON DELETE RESTRICT,
    is_active    BOOLEAN       NOT NULL DEFAULT TRUE,
    date_created TIMESTAMPTZ   NOT NULL,
    date_updated TIMESTAMPTZ   NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_products_category_id ON products(category_id);
