import re
import sys

regex = "^(build|chore|ci|docs|feat|fix|perf|refactor|revert|style|test){1}(\([\w\-\.]+\))?(!)?: ([\w ])+([\s\S]*)"

commit_message = sys.argv[1]
print(f'Commit message: "{commit_message}"')

match = re.search(regex, commit_message)

if match is None:
  print("The commit message does not follow the Conventional Commits specification.")
  exit(1)