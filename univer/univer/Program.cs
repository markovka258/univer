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
            Console.WriteLine("13. Удалить оценку");
            Console.WriteLine("14. Получить список студентов по факультету");
            Console.WriteLine("15. Получить список курсов, читаемых определенным преподавателем");
            Console.WriteLine("16. Получить список студентов, зачисленных на конкретный курс");
            Console.WriteLine("17. Получить оценки студентов по определенному курсу");
            Console.WriteLine("18. Средний балл студента по определенному курсу");
            Console.WriteLine("19. Средний балл студента в целом");
            Console.WriteLine("20. Средний балл по факультету");
            Console.WriteLine("21. Выход");

            try
            {
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
                        db.DeleteGrade();
                        break;
                    case 14:
                        db.GetStudentsByDepartment();
                        break;
                    case 15:
                        db.GetCoursesByTeacher();
                        break;
                    case 16:
                        db.GetStudentsByCourse();
                        break;
                    case 17:
                        db.GetGradesByCourse();
                        break;
                    case 18:
                        db.GetAverageGradeByCourse();
                        break;
                    case 19:
                        db.GetAverageGradeByStudent();
                        break;
                    case 20:
                        db.GetAverageGradeByFaculty();
                        break;
                    case 21:
                        db.Close();
                        return;
                    default:
                        Console.WriteLine("Некорректный выбор. Пожалуйста, попробуйте снова.");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Некорректный формат ввода. Пожалуйста, введите число.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
    }

    private void CreateTables()
    {
        string query = @"
            CREATE TABLE IF NOT EXISTS Students (
                student_id INTEGER PRIMARY KEY AUTOINCREMENT,
                student_name TEXT NOT NULL,
                student_surname TEXT NOT NULL,
                student_department TEXT NOT NULL,
                student_DB TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Teachers (
                teacher_id INTEGER PRIMARY KEY AUTOINCREMENT,
                teacher_name TEXT NOT NULL,
                teacher_surname TEXT NOT NULL,
                teacher_department TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Courses (
                course_id INTEGER PRIMARY KEY AUTOINCREMENT,
                title TEXT NOT NULL,
                description TEXT NOT NULL,
                teacher_id INTEGER NOT NULL,
                FOREIGN KEY(teacher_id) REFERENCES Teachers(teacher_id)
            );

            CREATE TABLE IF NOT EXISTS Exams (
                exam_id INTEGER PRIMARY KEY AUTOINCREMENT,
                date TEXT NOT NULL,
                course_id INTEGER NOT NULL,
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

        try
        {
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при создании таблиц: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void AddStudent()
    {
        try
        {
            Console.Write("Введите имя студента: ");
            string student_name = Console.ReadLine();
            Console.Write("Введите фамилию студента: ");
            string student_surname = Console.ReadLine();
            Console.Write("Введите факультет студента: ");
            string student_department = Console.ReadLine();
            Console.Write("Введите дату рождения студента: ");
            string student_DB = Console.ReadLine();

            string query = "INSERT INTO Students (student_name, student_surname, student_department, student_DB) VALUES (@student_name, @student_surname, @student_department, @student_DB)";

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
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при добавлении студента: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void AddTeacher()
    {
        try
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
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при добавлении учителя: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void AddCourse()
    {
        try
        {
            Console.Write("Введите название курса: ");
            string title = Console.ReadLine();
            Console.Write("Введите описание курса: ");
            string description = Console.ReadLine();
            Console.Write("Введите id преподавателя: ");
            int teacher_id = int.Parse(Console.ReadLine());

            // Проверка существования преподавателя
            string checkQuery = "SELECT COUNT(*) FROM Teachers WHERE teacher_id = @teacher_id";
            using (SQLiteCommand checkCommand = new SQLiteCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@teacher_id", teacher_id);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count == 0)
                {
                    Console.WriteLine("Преподаватель с указанным id не найден. Курс не добавлен.");
                    return; // Выход из метода, если преподаватель не найден
                }
            }

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
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат id преподавателя.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при добавлении курса: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void AddExam()
    {
        try
        {
            Console.Write("Введите дату экзамена: ");
            string date = Console.ReadLine();
            Console.Write("Введите id курса: ");
            int course_id = int.Parse(Console.ReadLine());

            // Проверка существования курса
            string checkQuery = "SELECT COUNT(*) FROM Courses WHERE course_id = @course_id";
            using (SQLiteCommand checkCommand = new SQLiteCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@course_id", course_id);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count == 0)
                {
                    Console.WriteLine("Курс с указанным id не найден. Экзамен не добавлен.");
                    return; // Выход из метода, если курс не найден
                }
            }

            string query = "INSERT INTO Exams (date, course_id) VALUES (@date, @course_id)";

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@course_id", course_id);

                command.ExecuteNonQuery();
                Console.WriteLine($"Экзамен {date} добавлен.");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат id курса.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при добавлении экзамена: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void AddGrade()
    {
        try
        {
            Console.Write("Введите id студента: ");
            int student_id = int.Parse(Console.ReadLine());
            Console.Write("Введите id экзамена: ");
            int exam_id = int.Parse(Console.ReadLine());
            Console.Write("Введите оценку: ");
            int score = int.Parse(Console.ReadLine());

            // Проверка существования студента
            string checkStudentQuery = "SELECT COUNT(*) FROM Students WHERE student_id = @student_id";
            using (SQLiteCommand checkStudentCommand = new SQLiteCommand(checkStudentQuery, connection))
            {
                checkStudentCommand.Parameters.AddWithValue("@student_id", student_id);
                int studentCount = Convert.ToInt32(checkStudentCommand.ExecuteScalar());

                if (studentCount == 0)
                {
                    Console.WriteLine("Студент с указанным id не найден. Оценка не добавлена.");
                    return; // Выход из метода, если студент не найден
                }
            }

            // Проверка существования экзамена
            string checkExamQuery = "SELECT COUNT(*) FROM Exams WHERE exam_id = @exam_id";
            using (SQLiteCommand checkExamCommand = new SQLiteCommand(checkExamQuery, connection))
            {
                checkExamCommand.Parameters.AddWithValue("@exam_id", exam_id);
                int examCount = Convert.ToInt32(checkExamCommand.ExecuteScalar());

                if (examCount == 0)
                {
                    Console.WriteLine("Экзамен с указанным id не найден. Оценка не добавлена.");
                    return; // Выход из метода, если экзамен не найден
                }
            }

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
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат id студента, id экзамена или оценки.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при добавлении оценки: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void UpdateStudent()
    {
        try
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

            string query = "UPDATE Students SET student_name = @student_name, student_surname = @student_surname, student_department = @student_department, student_DB = @student_DB WHERE student_id = @student_id";

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@student_name", student_name);
                command.Parameters.AddWithValue("@student_surname", student_surname);
                command.Parameters.AddWithValue("@student_department", student_department);
                command.Parameters.AddWithValue("@student_DB", student_DB);
                command.Parameters.AddWithValue("@student_id", student_id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Информация о студенте обновлена.");
                }
                else
                {
                    Console.WriteLine("Студент не найден.");
                }
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат id студента.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при обновлении студента: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void UpdateTeacher()
    {
        try
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
                command.Parameters.AddWithValue("@teacher_name", teacher_name);
                command.Parameters.AddWithValue("@teacher_surname", teacher_surname);
                command.Parameters.AddWithValue("@teacher_department", teacher_department);
                command.Parameters.AddWithValue("@teacher_id", teacher_id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Информация о преподавателе обновлена.");
                }
                else
                {
                    Console.WriteLine("Преп даватель не найден.");
                }
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат id преподавателя.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при обновлении преподавателя: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void UpdateCourse()
    {
        try
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
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@teacher_id", teacher_id);
                command.Parameters.AddWithValue("@course_id", course_id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Информация о курсе обновлена.");
                }
                else
                {
                    Console.WriteLine("Курс не найден.");
                }
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат id курса или id преподавателя.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при обновлении курса: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void DeleteStudent()
    {
        try
        {
            Console.Write("Введите id студента: ");
            int student_id = int.Parse(Console.ReadLine());

            string query = "DELETE FROM Students WHERE student_id = @student_id";

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@student_id", student_id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Студент удален.");
                }
                else
                {
                    Console.WriteLine("Студент не найден.");
                }
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат id студента.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при удалении студента: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void DeleteTeacher()
    {
        try
        {
            Console.Write("Введите id преподавателя: ");
            int teacher_id = int.Parse(Console.ReadLine());

            string query = "DELETE FROM Teachers WHERE teacher_id = @teacher_id";

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@teacher_id", teacher_id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Преподаватель удален.");
                }
                else
                {
                    Console.WriteLine("Преподаватель не найден.");
                }
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат id преподавателя.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при удалении преподавателя: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void DeleteCourse()
    {
        try
        {
            Console.Write("Введите id курса: ");
            int course_id = int.Parse(Console.ReadLine());

            string query = "DELETE FROM Courses WHERE course_id = @course_id";

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@course_id", course_id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Курс удален.");
                }
                else
                {
                    Console.WriteLine("Курс не найден.");
                }
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат id курса.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при удалении курса: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void DeleteExam()
    {
        try
        {
            Console.Write("Введите id экзамена: ");
            int exam_id = int.Parse(Console.ReadLine());

            string query = "DELETE FROM Exams WHERE exam_id = @exam_id";

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@exam_id", exam_id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Экзамен удален.");
                }
                else
                {
                    Console.WriteLine("Экзамен не найден.");
                }
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат id экзамена.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при удалении экзамена: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void DeleteGrade()
    {
        try
        {
            Console.Write("Введите id оценки: ");
            int grade_id = int.Parse(Console.ReadLine());

            // Сначала проверяем, существует ли оценка с данным id
            string checkQuery = "SELECT COUNT(*) FROM Grades WHERE grade_id = @grade_id";
            using (SQLiteCommand checkCommand = new SQLiteCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@grade_id", grade_id);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count == 0)
                {
                    Console.WriteLine("Оценка не найдена.");
                    return; // Выходим из метода, если оценка не найдена
                }
            }

            // Если оценка существует, удаляем ее
            string query = "DELETE FROM Grades WHERE grade_id = @grade_id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@grade_id", grade_id);
                command.ExecuteNonQuery();
                Console.WriteLine($"Оценка с id {grade_id} удалена.");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат id оценки.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при удалении оценки: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void GetStudentsByDepartment()
    {
        try
        {
            Console.Write("Введите факультет: ");
            string department = Console.ReadLine();

            // Проверка существования факультета
            string checkQuery = "SELECT COUNT(*) FROM Students WHERE student_department = @department";
            using (SQLiteCommand checkCommand = new SQLiteCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@department", department);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count == 0)
                {
                    Console.WriteLine("Факультет не найден. Студенты не могут быть получены.");
                    return; // Выход из метода, если факультет не найден
                }
            }

            // Запрос студентов по факультету
            string query = "SELECT * FROM Students WHERE student_department = @department";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@department", department);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine("Студенты по факультету:");
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["student_id"]}, Имя: {reader["student_name"]}, Фамилия: {reader["student_surname"]}, Факультет: {reader["student_department"]}, Дата рождения: {reader["student_DB"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Студенты не найдены.");
                    }
                }
            }
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при получении студентов по факультету: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void GetCoursesByTeacher()
    {
        try
        {
            Console.Write("Введите id преподавателя: ");
            int teacher_id = int.Parse(Console.ReadLine());

            // Проверка существования преподавателя
            string checkQuery = "SELECT COUNT(*) FROM Teachers WHERE teacher_id = @teacher_id";
            using (SQLiteCommand checkCommand = new SQLiteCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@teacher_id", teacher_id);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count == 0)
                {
                    Console.WriteLine("Преподаватель с указанным id не найден. Курсы не могут быть получены.");
                    return; // Выход из метода, если преподаватель не найден
                }
            }

            // Запрос курсов по id преподавателя
            string query = "SELECT * FROM Courses WHERE teacher_id = @teacher_id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@teacher_id", teacher_id);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine("Курсы преподавателя:");
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["course_id"]}, Название: {reader["title"]}, Описание: {reader["description"]}, ID преподавателя: {reader["teacher_id"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Курсы не найдены для данного преподавателя.");
                    }
                }
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат id преподавателя.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при получении курсов преподавателя: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void GetStudentsByCourse()
    {
        try
        {
            Console.Write("Введите id курса: ");
            int course_id = int.Parse(Console.ReadLine());

            // Проверка существования курса
            string checkQuery = "SELECT COUNT(*) FROM Courses WHERE course_id = @course_id";
            using (SQLiteCommand checkCommand = new SQLiteCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@course_id", course_id);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count == 0)
                {
                    Console.WriteLine("Курс с указанным id не найден. Студенты не могут быть получены.");
                    return; // Выход из метода, если курс не найден
                }
            }

            // Запрос студентов по курсу
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
                        Console.WriteLine("Студенты не найдены для данного курса.");
                    }
                }
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат id курса.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при получении студентов по курсу: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void GetGradesByCourse()
    {
        try
        {
            Console.Write("Введите id курса: ");
            int course_id = int.Parse(Console.ReadLine());

            // Проверка существования курса
            string checkCourseQuery = "SELECT COUNT(*) FROM Courses WHERE course_id = @course_id";
            using (SQLiteCommand checkCourseCommand = new SQLiteCommand(checkCourseQuery, connection))
            {
                checkCourseCommand.Parameters.AddWithValue("@course_id", course_id);
                int courseCount = Convert.ToInt32(checkCourseCommand.ExecuteScalar());

                if (courseCount == 0)
                {
                    Console.WriteLine("Курс с указанным id не найден. Оценки не могут быть получены.");
                    return; // Выход из метода, если курс не найден
                }
            }

            // Запрос оценок по курсу
            string query = @"
            SELECT s.student_id, s.student_name, g.score
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
                            Console.WriteLine($"Студент {reader["student_name"]} имеет оценку {reader["score"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Оценки не найдены для данного курса.");
                    }
                }
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат id курса.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при получении оценок по курсу: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void GetAverageGradeByCourse()
    {
        try
        {
            Console.Write("Введите id курса: ");
            int course_id = int.Parse(Console.ReadLine());

            // Проверка существования курса
            string checkCourseQuery = "SELECT COUNT(*) FROM Courses WHERE course_id = @course_id";
            using (SQLiteCommand checkCourseCommand = new SQLiteCommand(checkCourseQuery, connection))
            {
                checkCourseCommand.Parameters.AddWithValue("@course_id", course_id);
                int courseCount = Convert.ToInt32(checkCourseCommand.ExecuteScalar());

                if (courseCount == 0)
                {
                    Console.WriteLine("Курс с указанным id не найден. Средний балл не может быть получен.");
                    return; // Выход из метода, если курс не найден
                }
            }

            // Запрос среднего балла по курсу
            string query = @"
            SELECT AVG(g.score) AS average_score
            FROM Grades g
            JOIN Exams e ON g.exam_id = e.exam_id
            WHERE e.course_id = @course_id
        ";

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@course_id", course_id);

                object result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    double averageScore = Convert.ToDouble(result);
                    Console.WriteLine($"Средний балл по курсу: {averageScore}");
                }
                else
                {
                    Console.WriteLine("Оценки не найдены для данного курса.");
                }
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат id курса.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при получении среднего балла по курсу: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void GetAverageGradeByStudent()
    {
        try
        {
            Console.Write("Введите id студента: ");
            int student_id = int.Parse(Console.ReadLine());

            string query = @"
                SELECT AVG(g.score) AS average_grade
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
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат id студента.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при получении среднего балла студента: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void GetAverageGradeByFaculty()
    {
        try
        {
            Console.Write("Введите id факультета: ");
            int student_department = int.Parse(Console.ReadLine());

            string query = @"
                SELECT AVG(g.score) AS average_grade
                FROM Grades g
                JOIN Students s ON g.student_id = s.student_id
                WHERE s.student_department = @student_department
            ";

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@student_department", student_department);

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
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Некорректный формат id факультета.");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Ошибка при получении среднего балла по факультету: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    public void Close()
    {
        connection.Close();
    }


}