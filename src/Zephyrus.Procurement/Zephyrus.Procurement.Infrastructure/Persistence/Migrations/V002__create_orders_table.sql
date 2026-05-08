CREATE TABLE IF NOT EXISTS orders
(
    id                   UUID           PRIMARY KEY,
    supplier_id          UUID           NOT NULL,
    total_price          NUMERIC(18, 2) NOT NULL,
    status               SMALLINT       NOT NULL DEFAULT 1,
    created_by           UUID           NOT NULL,
    date_created         TIMESTAMPTZ    NOT NULL,
    date_updated         TIMESTAMPTZ    NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_orders_supplier_id ON orders(supplier_id);
CREATE INDEX IF NOT EXISTS idx_orders_status ON orders(status);