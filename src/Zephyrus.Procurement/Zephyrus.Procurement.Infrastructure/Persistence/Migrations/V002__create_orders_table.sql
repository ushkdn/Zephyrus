CREATE TABLE IF NOT EXISTS orders
(
    id                   UUID           PRIMARY KEY,
    purchase_request_id  UUID           NOT NULL REFERENCES purchase_requests(id) ON DELETE RESTRICT,
    supplier_id          UUID           NOT NULL,
    product_id           UUID           NOT NULL,
    quantity             NUMERIC(18, 3) NOT NULL,
    unit_price           NUMERIC(18, 2) NOT NULL,
    currency             VARCHAR(10)    NOT NULL,
    total_price          NUMERIC(18, 2) NOT NULL,
    status               SMALLINT       NOT NULL DEFAULT 1,
    created_by           UUID           NOT NULL,
    date_created         TIMESTAMPTZ    NOT NULL,
    date_updated         TIMESTAMPTZ    NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_orders_purchase_request_id ON orders(purchase_request_id);
CREATE INDEX IF NOT EXISTS idx_orders_supplier_id ON orders(supplier_id);
CREATE INDEX IF NOT EXISTS idx_orders_status ON orders(status);
