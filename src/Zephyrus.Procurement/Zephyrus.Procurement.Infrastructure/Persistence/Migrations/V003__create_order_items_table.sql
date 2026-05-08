CREATE TABLE IF NOT EXISTS order_items
(
    id                   UUID           PRIMARY KEY,
    order_id             UUID           NOT NULL REFERENCES orders(id) ON DELETE CASCADE,
    purchase_request_id  UUID           NOT NULL REFERENCES purchase_requests(id) ON DELETE RESTRICT,
    unit_price           NUMERIC(18, 2) NOT NULL,
    currency             VARCHAR(10)    NOT NULL,
    total_price          NUMERIC(18, 2) NOT NULL,
    date_created         TIMESTAMPTZ    NOT NULL,
    date_updated         TIMESTAMPTZ    NOT NULL
    );

CREATE INDEX IF NOT EXISTS idx_order_items_order_id ON order_items(order_id);
CREATE INDEX IF NOT EXISTS idx_order_items_purchase_request_id ON order_items(purchase_request_id);