CREATE TABLE IF NOT EXISTS purchase_requests
(
    id           UUID           PRIMARY KEY,
    product_id   UUID           NOT NULL,
    quantity     NUMERIC(18, 3) NOT NULL,
    unit         VARCHAR(20)    NOT NULL,
    requested_by UUID           NOT NULL,
    status       SMALLINT       NOT NULL DEFAULT 1,
    comment      TEXT,
    date_created TIMESTAMPTZ    NOT NULL,
    date_updated TIMESTAMPTZ    NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_purchase_requests_requested_by ON purchase_requests(requested_by);
CREATE INDEX IF NOT EXISTS idx_purchase_requests_status ON purchase_requests(status);
