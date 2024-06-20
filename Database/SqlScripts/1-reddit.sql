CREATE TABLE
  reddit_posts (id INTEGER PRIMARY KEY, post jsonb NOT NULL);

CREATE TABLE
  reddit_data (
    id SERIAL PRIMARY KEY,
    post_id INTEGER REFERENCES reddit_posts (id),
    messages jsonb NOT NULL
  );