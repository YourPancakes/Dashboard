import { useEffect, useState } from 'react'
import {
  getClients,
  getRate,
  createClient,
  updateClient,
  deleteClient,
  updateRate,
} from '../api'
import type { ClientDto, RateDto } from '../types'
import { ClientTable } from '../components/ClientTable'
import { ClientForm } from '../components/ClientForm'
import { PaymentHistory } from '../components/PaymentHistory'
import './DashboardPage.css'

export function DashboardPage() {
  useEffect(() => { document.title = 'Dashboard' }, [])

  const [clients, setClients] = useState<ClientDto[]>([])
  const [rate, setRate] = useState<RateDto | null>(null)
  const [newRate, setNewRate] = useState(0)
  const [showForm, setShowForm] = useState(false)
  const [editingClient, setEditingClient] = useState<ClientDto | null>(null)
  const [historyClientId, setHistoryClientId] = useState<number | null>(null)

  const fetchData = async () => {
    const [c, r] = await Promise.all([getClients(), getRate()])
    setClients(c)
    setRate(r)
    setNewRate(r.currentRate)
  }

  useEffect(() => { fetchData() }, [])

  const handleAdd = () => { setEditingClient(null); setShowForm(true) }
  const handleEdit = (c: ClientDto) => { setEditingClient(c); setShowForm(true) }
  const handleDelete = async (id: number) => {
    if (confirm('Delete client?')) {
      await deleteClient(id)
      await fetchData()
    }
  }
  const handleSave = async (dto: { name: string; email: string; balanceT: number }) => {
    if (editingClient) await updateClient(editingClient.id, dto)
    else await createClient(dto)
    setShowForm(false)
    await fetchData()
  }
  const handleHistory = (id: number) => { setHistoryClientId(id) }
  const closeHistory = () => { setHistoryClientId(null) }

  if (!rate) return <div>Loading dashboard...</div>

  return (
    <div className="dashboard-container">
      <div className="dashboard-header">
        <h1>Dashboard</h1>
        <button className="btn" onClick={handleAdd}>+ New Client</button>
      </div>

      <div className="card client-table">
        <ClientTable
          clients={clients}
          onEdit={handleEdit}
          onDelete={handleDelete}
          onHistory={handleHistory}
        />
      </div>

      {showForm && (
        <div className="overlay">
          <div className="modal">
            <button className="close-btn" onClick={() => setShowForm(false)}>&times;</button>
            <ClientForm
              client={editingClient ?? undefined}
              onSave={handleSave}
              onCancel={() => setShowForm(false)}
            />
          </div>
        </div>
      )}

      {historyClientId !== null && (
        <div className="overlay">
          <PaymentHistory clientId={historyClientId} onClose={closeHistory} />
        </div>
      )}

      <div className="card rate-block">
        <span className="rate-label">Current Rate:</span>
        <span className="rate-value">{rate.currentRate}</span>
        <input
          type="number"
          value={newRate}
          onChange={e => setNewRate(Number(e.target.value))}
        />
        <button onClick={async () => { await updateRate(newRate); fetchData() }}>
          Update Rate
        </button>
      </div>
    </div>
  )
}
