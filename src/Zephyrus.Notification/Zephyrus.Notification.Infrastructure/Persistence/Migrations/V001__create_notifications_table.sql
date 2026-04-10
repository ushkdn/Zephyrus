CREATE TABLE IF NOT EXISTS notifications
(
    id           UUID          PRIMARY KEY,
    recipient_id UUID          NOT NULL,
    title        VARCHAR(200)  NOT NULL,
    message      VARCHAR(1000) NOT NULL,
    type         VARCHAR(100)  NOT NULL,
    is_read      BOOLEAN       NOT NULL DEFAULT false,
    date_created TIMESTAMPTZ   NOT NULL
);
