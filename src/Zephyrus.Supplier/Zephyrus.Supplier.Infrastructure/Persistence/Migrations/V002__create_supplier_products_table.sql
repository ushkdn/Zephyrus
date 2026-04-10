CREATE TABLE IF NOT EXISTS supplier_products
(
    id           UUID           PRIMARY KEY,
    supplier_id  UUID           NOT NULL REFERENCES suppliers(id) ON DELETE CASCADE,
    product_id   UUID           NOT NULL,
    price        NUMERIC(18, 2) NOT NULL,
    currency     VARCHAR(10)    NOT NULL,
    is_available BOOLEAN        NOT NULL DEFAULT TRUE,
    date_updated TIMESTAMPTZ    NOT NULL,
    CONSTRAINT uq_supplier_product UNIQUE (supplier_id, product_id)
);

CREATE INDEX IF NOT EXISTS idx_supplier_products_supplier_id ON supplier_products(supplier_id);
