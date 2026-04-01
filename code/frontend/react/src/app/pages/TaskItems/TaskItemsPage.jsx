import React, { useState, useEffect } from 'react';
import { TaskItemService } from '../../services/TaskItemService';
import { TaskItemModal } from '../../components/TaskItemModal/TaskItemModal';
import styles from './TaskItemsPage.module.css';

const HARDCODED_USER_ID = 'user123';

export function TaskItemsPage() {
  const [tasks, setTasks] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [editingTask, setEditingTask] = useState(null);

  // Load tasks on mount
  useEffect(() => {
    loadTasks();
  }, []);

  const loadTasks = async () => {
    setLoading(true);
    setError(null);
    try {
      const userTasks = await TaskItemService.getTaskItemsByUserId(HARDCODED_USER_ID);
      setTasks(userTasks);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load tasks');
    } finally {
      setLoading(false);
    }
  };

  const handleOpenModal = (task) => {
    setEditingTask(task || null);
    setShowModal(true);
  };

  const handleCloseModal = () => {
    setShowModal(false);
    setEditingTask(null);
  };

  const handleSaveTask = async (data) => {
    setError(null);
    try {
      if (editingTask) {
        // Update existing task
        const updatedTask = await TaskItemService.updateTaskItem(editingTask.id, data);
        setTasks(tasks.map(t => t.id === updatedTask.id ? updatedTask : t));
      } else {
        // Create new task
        const newTask = await TaskItemService.createTaskItem(data);
        setTasks([...tasks, newTask]);
      }
      handleCloseModal();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to save task');
    }
  };

  const handleDeleteTask = async (id) => {
    if (!window.confirm('Are you sure you want to delete this task?')) return;

    setError(null);
    try {
      await TaskItemService.deleteTaskItem(id);
      setTasks(tasks.filter(t => t.id !== id));
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to delete task');
    }
  };

  const handleToggleComplete = async (task) => {
    setError(null);
    try {
      const updatedTask = await TaskItemService.updateTaskItem(task.id, {
        title: task.title,
        description: task.description,
        isCompleted: !task.isCompleted,
      });
      setTasks(tasks.map(t => t.id === updatedTask.id ? updatedTask : t));
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to update task');
    }
  };

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1>Task Items</h1>
        <button className={styles.createButton} onClick={() => handleOpenModal()}>
          + Add Task
        </button>
      </div>

      {error && <div className={styles.error}>{error}</div>}

      {loading && <div className={styles.loading}>Loading tasks...</div>}

      {!loading && tasks.length === 0 && !error && (
        <div className={styles.empty}>No tasks yet. Create one to get started!</div>
      )}

      {!loading && tasks.length > 0 && (
        <div className={styles.taskList}>
          {tasks.map(task => (
            <div key={task.id} className={`${styles.taskItem} ${task.isCompleted ? styles.completed : ''}`}>
              <div className={styles.taskContent}>
                <input
                  type="checkbox"
                  checked={task.isCompleted}
                  onChange={() => handleToggleComplete(task)}
                  className={styles.checkbox}
                />
                <div className={styles.taskDetails}>
                  <h3 className={styles.title}>{task.title}</h3>
                  <p className={styles.description}>{task.description}</p>
                  <small className={styles.meta}>
                    {new Date(task.createdAt).toLocaleDateString()}
                  </small>
                </div>
              </div>
              <div className={styles.actions}>
                <button
                  className={styles.editButton}
                  onClick={() => handleOpenModal(task)}
                  title="Edit task"
                >
                  ✎
                </button>
                <button
                  className={styles.deleteButton}
                  onClick={() => handleDeleteTask(task.id)}
                  title="Delete task"
                >
                  ✕
                </button>
              </div>
            </div>
          ))}
        </div>
      )}

      {showModal && (
        <TaskItemModal
          task={editingTask}
          userId={HARDCODED_USER_ID}
          onSave={handleSaveTask}
          onClose={handleCloseModal}
        />
      )}
    </div>
  );
}
