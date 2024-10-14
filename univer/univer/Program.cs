using System;
using System.Data.SQLite;

class Program
{
    private SQLiteConnection connection;

    public Program()
    {
        connection = new SQLiteConnection("Data Source=university.db");
        connection.Open();
        CreateTables();
    }

    public static void Main(string[] args)
    {
        Program db = new Program();

        while (true)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Добавить преподавателя");
            Console.WriteLine("3. Добавить курс");
            Console.WriteLine("4. Добавить экзамен");
            Console.WriteLine("5. Добавить оценку");
            Console.WriteLine("6. Изменить информацию о студенте");
            Console.WriteLine("7. Изменить информацию о преподавателе");
            Console.WriteLine("8. Изменить информацию о курсе");
            Console.WriteLine("9. Удалить студента");
            Console.WriteLine("10. Удалить преподавателя");
            Console.WriteLine("11. Удалить курс");
            Console.WriteLine("12. Удалить экзамен");
            Console.WriteLine("13. Получить список студентов по факультету");
            Console.WriteLine("14. Получить список курсов, читаемых определенным преподавателем");
            Console.WriteLine("15. Получить список студентов, зачисленных на конкретный курс");
            Console.WriteLine("16. Получить оценки студентов по определенному курсу");
            Console.WriteLine("17. Средний балл студента по определенному курсу");
            Console.WriteLine("18. Средний балл студента в целом");
            Console.WriteLine("19. Средний балл по факультету");
            Console.WriteLine("20. Выход");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    db.AddStudent();
                    break;
                case 2:
                    db.AddTeacher();
                    break;
                case 3:
                    db.AddCourse();
                    break;
                case 4:
                    db.AddExam();
                    break;
                case 5:
                    db.AddGrade();
                    break;
                case 6:
                    db.UpdateStudent();
                    break;
                case 7:
                    db.UpdateTeacher();
                    break;
                case 8:
                    db.UpdateCourse();
                    break;
                case 9:
                    db.DeleteStudent();
                    break;
                case 10:
                    db.DeleteTeacher();
                    break;
                case 11:
                    db.DeleteCourse();
                    break;
                case 12:
                    db.DeleteExam();
                    break;
                case 13:
                    db.GetStudentsByDepartment();
                    break;
                case 14:
                    db.GetCoursesByTeacher();
                    break;
                case 15: 
                    db.GetStudentsByCourse();
                    break;
                case 16:
                    db.GetGradesByCourse();
                    break;
                case 17:
                    db.GetAverageGradeByCourse();
                    break;
                case 18:
                    db.GetAverageGradeByStudent();
                    break;
                case 19:
                    db.GetAverageGradeByFaculty();
                    break;
                case 20:
                    return;
            }
        }
    }

    private void CreateTables()
    {
        string query = @"
            CREATE TABLE IF NOT EXISTS Students (
                student_id INTEGER PRIMARY KEY AUTOINCREMENT,   -- id ученика, первичный ключ с автоинкрементом
                student_name TEXT NOT NULL,                     -- Имя ученика
                student_surname TEXT NOT NULL,                  -- Фамилия ученика
                student_department TEXT NOT NULL,               -- Факультет ученика
                srudent_DB TEXT NOT NULL                        -- Дата рождения ученика
            );

            CREATE TABLE IF NOT EXISTS Teachers(
                teacher_id INTEGER PRIMARY KEY AUTOINCREMENT,   --id преподавателя, первичный ключ с автоинкрементом
                teacher_name TEXT NOT NULL,                     --Имя преподавателя
                teacher_surname TEXT NOT NULL,                  --Фамилия преподавателя
                teacher_department TEXT NOT NULL-- Факультет преподавателя
            );

            CREATE TABLE IF NOT EXISTS Courses(
                course_id INTEGER PRIMARY KEY AUTOINCREMENT,     --id курса, первичный ключ с автоинкрементом
                title TEXT NOT NULL,                             --название курса
                description TEXT NOT NULL,                       --описание курса
                teacher_id INTEGER NOT NULL,                     --id преподавателя
                FOREIGN KEY(teacher_id) REFERENCES Teachers(teacher_id)
            );

            CREATE TABLE IF NOT EXISTS Exams (
                exam_id INTEGER PRIMARY KEY AUTOINCREMENT,           -- id экзамена, первичный ключ с автоинкрементом
                date TEXT NOT NULL,                                  -- Дата
                course_id INTEGER NOT NULL,                         --id курса
                FOREIGN KEY (course_id) REFERENCES Courses(course_id)
            );

            CREATE TABLE IF NOT EXISTS Grades (
                grade_id INTEGER PRIMARY KEY AUTOINCREMENT,
                student_id INTEGER NOT NULL,
                exam_id INTEGER NOT NULL,
                score INTEGER NOT NULL,
                FOREIGN KEY (student_id) REFERENCES Students(student_id),
                FOREIGN KEY (exam_id) REFERENCES Exams(exam_id)
            );
        ";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
    }

    public void AddStudent()
    {
        Console.Write("Введите имя студента: ");
        string student_name = Console.ReadLine();
        Console.Write("Введите фамилию студента: ");
        string student_surname = Console.ReadLine();
        Console.Write("Введите факультет студента: ");
        string student_department = Console.ReadLine();
        Console.Write("Введите дату рождения студента: ");
        string student_DB = Console.ReadLine();

        string query = "INSERT INTO Students (student_name, student_surname, student_department, srudent_DB) VALUES (@student_name, @student_surname, @student_department, @student_DB)";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@student_name", student_name);
            command.Parameters.AddWithValue("@student_surname", student_surname);
            command.Parameters.AddWithValue("@student_department", student_department);
            command.Parameters.AddWithValue("@student_DB", student_DB);

            command.ExecuteNonQuery();
            Console.WriteLine($"Ученик {student_surname} {student_name} добавлен.");
        }
    }

    public void AddTeacher()
    {
        Console.Write("Введите имя преподавателя: ");
        string teacher_name = Console.ReadLine();
        Console.Write("Введите фамилию преподавателя: ");
        string teacher_surname = Console.ReadLine();
        Console.Write("Введите факультет преподавателя: ");
        string teacher_department = Console.ReadLine();

        string query = "INSERT INTO Teachers (teacher_name, teacher_surname, teacher_department) VALUES (@teacher_name, @teacher_surname, @teacher_department)";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@teacher_name", teacher_name);
            command.Parameters.AddWithValue("@teacher_surname", teacher_surname);
            command.Parameters.AddWithValue("@teacher_department", teacher_department);

            command.ExecuteNonQuery();
            Console.WriteLine($"Учитель {teacher_name} {teacher_surname} добавлен.");
        }
    }

    public void AddCourse()
    {
        Console.Write("Введите название курса: ");
        string title = Console.ReadLine();
        Console.Write("Введите описание курса: ");
        string description = Console.ReadLine();
        Console.Write("Введите id преподавателя: ");
        int teacher_id = int.Parse(Console.ReadLine());

        string query = "INSERT INTO Courses (title, description, teacher_id) VALUES (@title, @description, @teacher_id)";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@title", title);
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@teacher_id", teacher_id);

            command.ExecuteNonQuery();
            Console.WriteLine($"Курс '{title}' добавлен.");
        }
    }

    public void AddExam()
    {
        Console.Write("Введите дату экзамена: ");
        string date = Console.ReadLine();
        Console.Write("Введите id курса: ");
        int course_id = int.Parse(Console.ReadLine());

        string query = "INSERT INTO Exams (date, course_id) VALUES (@date, @course_id)";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@course_id", course_id);

            command.ExecuteNonQuery();
            Console.WriteLine($"Экзамен {date} добавлен.");
        }
    }

    public void AddGrade()
    {
        Console.Write("Введите id студента: ");
        int student_id = int.Parse(Console.ReadLine());
        Console.Write("Введите id экзамена: ");
        int exam_id = int.Parse(Console.ReadLine());
        Console.Write("Введите оценку: ");
        int score = int.Parse(Console.ReadLine());

        string query = "INSERT INTO Grades (student_id, exam_id, score) VALUES (@student_id, @exam_id, @score)";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@student_id", student_id);
            command.Parameters.AddWithValue("@exam_id", exam_id);
            command.Parameters.AddWithValue("@score", score);

            command.ExecuteNonQuery();
            Console.WriteLine($"Оценка добавлена.");
        }
    }

    public void UpdateStudent()
    {
        Console.Write("Введите id студента: ");
        int student_id = int.Parse(Console.ReadLine());
        Console.Write("Введите новое имя студента: ");
        string student_name = Console.ReadLine();
        Console.Write("Введите новую фамилию студента: ");
        string student_surname = Console.ReadLine();
        Console.Write("Введите новый факультет студента: ");
        string student_department = Console.ReadLine();
        Console.Write("Введите новую дату рождения студента: ");
        string student_DB = Console.ReadLine();

        string query = "UPDATE Students SET student_name = @student_name, student_surname = @student_surname, student_department = @student_department, srudent_DB = @student_DB WHERE student_id = @student_id";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@student_id", student_id);

            object result = command.ExecuteScalar();

            if (result != null)
            {
                query = "UPDATE Students SET student_name = @student_name, student_surname = @student_surname, student_department = @student_department, srudent_DB = @student_DB WHERE student_id = @student_id";

                using (SQLiteCommand updateCommand = new SQLiteCommand(query, connection))
                {
                    updateCommand.Parameters.AddWithValue("@student_name", student_name);
                    updateCommand.Parameters.AddWithValue("@student_surname", student_surname);
                    updateCommand.Parameters.AddWithValue("@student_department", student_department);
                    updateCommand.Parameters.AddWithValue("@student_DB", student_DB);
                    updateCommand.Parameters.AddWithValue("@student_id", student_id);

                    updateCommand.ExecuteNonQuery();
                    Console.WriteLine($"Информация о студенте обновлена.");
                }
            }
            else
            {
                Console.WriteLine("Студент не найден.");
            }
        }
    }

    public void UpdateTeacher()
    {
        Console.Write("Введите id преподавателя: ");
        int teacher_id = int.Parse(Console.ReadLine());
        Console.Write("Введите новое имя преподавателя: ");
        string teacher_name = Console.ReadLine();
        Console.Write("Введите новую фамилию преподавателя: ");
        string teacher_surname = Console.ReadLine();
        Console.Write("Введите новый факультет преподавателя: ");
        string teacher_department = Console.ReadLine();

        string query = "UPDATE Teachers SET teacher_name = @teacher_name, teacher_surname = @teacher_surname, teacher_department = @teacher_department WHERE teacher_id = @teacher_id";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@teacher_id", teacher_id);

            object result = command.ExecuteScalar();

            if (result != null)
            {
                query = "UPDATE Teachers SET teacher_name = @teacher_name, teacher_surname = @teacher_surname, teacher_department = @teacher_department WHERE teacher_id = @teacher_id";

                using (SQLiteCommand updateCommand = new SQLiteCommand(query, connection))
                {
                    updateCommand.Parameters.AddWithValue("@teacher_name", teacher_name);
                    updateCommand.Parameters.AddWithValue("@teacher_surname", teacher_surname);
                    updateCommand.Parameters.AddWithValue("@teacher_department", teacher_department);
                    updateCommand.Parameters.AddWithValue("@teacher_id", teacher_id);

                    updateCommand.ExecuteNonQuery();
                    Console.WriteLine($"Информация о преподавателе обновлена.");
                }
            }
            else
            {
                Console.WriteLine("Преподаватель не найден.");
            }
        }
    }

    public void UpdateCourse()
    {
        Console.Write("Введите id курса: ");
        int course_id = int.Parse(Console.ReadLine());
        Console.Write("Введите новое название курса: ");
        string title = Console.ReadLine();
        Console.Write("Введите новое описание курса: ");
        string description = Console.ReadLine();
        Console.Write("Введите новый id преподавателя: ");
        int teacher_id = int.Parse(Console.ReadLine());

        string query = "UPDATE Courses SET title = @title, description = @description, teacher_id = @teacher_id WHERE course_id = @course_id";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@course_id", course_id);

            object result = command.ExecuteScalar();

            if (result != null)
            {
                query = "UPDATE Courses SET title = @title, description = @description, teacher_id = @teacher_id WHERE course_id = @course_id";

                using (SQLiteCommand updateCommand = new SQLiteCommand(query, connection))
                {
                    updateCommand.Parameters.AddWithValue("@title", title);
                    updateCommand.Parameters.AddWithValue("@description", description);
                    updateCommand.Parameters.AddWithValue("@teacher_id", teacher_id);
                    updateCommand.Parameters.AddWithValue("@course_id", course_id);

                    updateCommand.ExecuteNonQuery();
                    Console.WriteLine($"Информация о курсе обновлена.");
                }
            }
            else
            {
                Console.WriteLine("Курс не найден.");
            }
        }
    }

    public void DeleteStudent()
    {
        Console.Write("Введите id студента: ");
        int student_id = int.Parse(Console.ReadLine());

        string query = "DELETE FROM Students WHERE student_id = @student_id";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@student_id", student_id);

            object result = command.ExecuteScalar();

            if (result != null)
            {
                query = "DELETE FROM Students WHERE student_id = @student_id";

                using (SQLiteCommand deleteCommand = new SQLiteCommand(query, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@student_id", student_id);

                    deleteCommand.ExecuteNonQuery();
                    Console.WriteLine($"Студент с id {student_id} удален.");
                }
            }
            else
            {
                Console.WriteLine("Студент не найден.");
            }
        }
    }

    public void DeleteTeacher()
    {
        Console.Write("Введите id преподавателя: ");
        int teacher_id = int.Parse(Console.ReadLine());

        string query = "DELETE FROM Teachers WHERE teacher_id = @teacher_id";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@teacher_id", teacher_id);

            object result = command.ExecuteScalar();

            if (result != null)
            {
                query = "DELETE FROM Teachers WHERE teacher_id = @teacher_id";

                using (SQLiteCommand deleteCommand = new SQLiteCommand(query, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@teacher_id", teacher_id);

                    deleteCommand.ExecuteNonQuery();
                    Console.WriteLine($"Преподаватель с id {teacher_id} удален.");
                }
            }
            else
            {
                Console.WriteLine("Преподаватель не найден.");
            }
        }
    }

    public void DeleteCourse()
    {
        Console.Write("Введите id курса: ");
        int course_id = int.Parse(Console.ReadLine());

        string query = "DELETE FROM Courses WHERE course_id = @course_id";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@course_id", course_id);

            object result = command.ExecuteScalar();

            if (result != null)
            {
                query = "DELETE FROM Courses WHERE course_id = @course_id";

                using (SQLiteCommand deleteCommand = new SQLiteCommand(query, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@course_id", course_id);

                    deleteCommand.ExecuteNonQuery();
                    Console.WriteLine($"Курс с id {course_id} удален.");
                }
            }
            else
            {
                Console.WriteLine("Курс не найден.");
            }
        }
    }

    public void DeleteExam()
    {
        Console.Write("Введите id экзамена: ");
        int exam_id = int.Parse(Console.ReadLine());

        string query = "DELETE FROM Exams WHERE exam_id = @exam_id";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@exam_id", exam_id);

            object result = command.ExecuteScalar();

            if (result != null)
            {
                query = "DELETE FROM Exams WHERE exam_id = @exam_id";

                using (SQLiteCommand deleteCommand = new SQLiteCommand(query, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@exam_id", exam_id);

                    deleteCommand.ExecuteNonQuery();
                    Console.WriteLine($"Экзамен с id {exam_id} удален.");
                }
            }
            else
            {
                Console.WriteLine("Экзамен не найден.");
            }
        }
    }

    public void GetStudentsByDepartment()
    {
        Console.Write("Введите факультет: ");
        string department = Console.ReadLine();

        string query = "SELECT * FROM Students WHERE student_department = @department";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@department", department);

            int count = (int)command.ExecuteScalar();

            if (count > 0)
            {
                query = "SELECT * FROM Students WHERE student_department = @department";

                using (SQLiteCommand selectCommand = new SQLiteCommand(query, connection))
                {
                    selectCommand.Parameters.AddWithValue("@department", department);

                    using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["student_id"]}, Имя: {reader["student_name"]}, Фамилия: {reader["student_surname"]}, Факультет: {reader["student_department"]}, Дата рождения: {reader["student_DB"]}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Студенты не найдены.");
            }
        }
    }

    public void GetCoursesByTeacher()
    {
        Console.Write("Введите id преподавателя: ");
        int teacher_id = int.Parse(Console.ReadLine());

        string query = "SELECT * FROM Courses WHERE teacher_id = @teacher_id";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@teacher_id", teacher_id);

            int count = (int)command.ExecuteScalar();

            if (count > 0)
            {
                query = "SELECT * FROM Courses WHERE teacher_id = @teacher_id";

                using (SQLiteCommand selectCommand = new SQLiteCommand(query, connection))
                {
                    selectCommand.Parameters.AddWithValue("@teacher_id", teacher_id);

                    using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["course_id"]}, Название: {reader["title"]}, Описание: {reader["description"]}, ID преподавателя: {reader["teacher_id"]}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Курсы не найдены.");
            }
        }
    }

    public void GetStudentsByCourse()
    {
        Console.Write("Введите id курса: ");
        int course_id;

        try
        {
            course_id = int.Parse(Console.ReadLine());
        }
        catch (FormatException ex)
        {
            Console.WriteLine("Ошибка: Некорректный формат id курса.");
            return;
        }

        string query = @"
            SELECT s.student_id, s.student_name, s.student_surname, s.student_department, s.student_DB
            FROM Students s
            JOIN Grades g ON s.student_id = g.student_id
            JOIN Exams e ON g.exam_id = e.exam_id
            WHERE e.course_id = @course_id
        ";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@course_id", course_id);

            try
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine("Студенты по курсу:");
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["student_id"]}, Имя: {reader["student_name"]}, Фамилия: {reader["student_surname"]}, Факультет: {reader["student_department"]}, Дата рождения: {reader["student_DB"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Студенты не найдены.");
                        return; 
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Ошибка: Ошибка при чтении данных из базы данных.");
            }
        }
    }

    public void GetGradesByCourse()
    {
        Console.Write("Введите id курса: ");
        int course_id = int.Parse(Console.ReadLine());

        string query = @"
        SELECT s.student_id, s.name, g.grade
        FROM Grades g
        JOIN Exams e ON g.exam_id = e.exam_id
        JOIN Students s ON g.student_id = s.student_id
        WHERE e.course_id = @course_id
    ";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@course_id", course_id);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Console.WriteLine("Оценки студентов по курсу:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"Студент {reader["name"]} имеет оценку {reader["grade"]}");
                    }
                }
                else
                {
                    Console.WriteLine("Нет оценок по этому курсу");
                }
            }
        }
    }

    public void GetAverageGradeByCourse()
    {
        Console.Write("Введите id студента: ");
        int student_id = int.Parse(Console.ReadLine());

        Console.Write("Введите id курса: ");
        int course_id = int.Parse(Console.ReadLine());

        string query = @"
        SELECT AVG(score) AS average_grade
        FROM Grades g
        JOIN Exams e ON g.exam_id = e.exam_id
        WHERE g.student_id = @student_id AND e.course_id = @course_id
    ";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@student_id", student_id);
            command.Parameters.AddWithValue("@course_id", course_id);

            object result = command.ExecuteScalar();

            if (result != null)
            {
                Console.WriteLine($"Средний балл студента по курсу: {result}");
            }
            else
            {
                Console.WriteLine("Студент не имеет оценок по этому курсу");
            }
        }
    }

    public void GetAverageGradeByStudent()
    {
        Console.Write("Введите id студента: ");
        int student_id = int.Parse(Console.ReadLine());

        string query = @"
        SELECT AVG(grade) AS average_grade
        FROM Grades g
        WHERE g.student_id = @student_id
    ";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@student_id", student_id);

            object result = command.ExecuteScalar();

            if (result != null)
            {
                Console.WriteLine($"Средний балл студента: {result}");
            }
            else
            {
                Console.WriteLine("Студент не имеет оценок");
            }
        }
    }

    public void GetAverageGradeByFaculty()
    {
        Console.Write("Введите id факультета: ");
        int faculty_id = int.Parse(Console.ReadLine());

        string query = @"
        SELECT AVG(g.grade) AS average_grade
        FROM Grades g
        JOIN Students s ON g.student_id = s.student_id
        WHERE s.faculty_id = @faculty_id
    ";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@faculty_id", faculty_id);

            object result = command.ExecuteScalar();

            if (result != null)
            {
                Console.WriteLine($"Средний балл по факультету: {result}");
            }
            else
            {
                Console.WriteLine("Факультет не имеет оценок");
            }
        }
    }
}